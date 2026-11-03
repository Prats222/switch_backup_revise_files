using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ElevatorSystem
{
    // 'Direction' converted to C# enum with idiomatic PascalCase names.
    public enum Direction
    {
        Up,
        Down,
        Idle
    }

    // Value object representing a floor request from the hallway or inside an elevator.
    public sealed class Request
    {
        public Request(int requestedFloor, Direction direction)
        {
            RequestedFloor = requestedFloor;
            Direction = direction;
        }

        public int RequestedFloor { get; }
        public Direction Direction { get; }
    }

    // Strategy interface for dispatching the best elevator for a request.
    public interface DispatchStrategy
    {
        Elevator SelectOptimalElevator(List<Elevator> elevators, Request request);
    }

    // Nearest Elevator strategy: select elevator that is idle or already moving toward the request.
    public sealed class NearestElevatorStrategy : DispatchStrategy
    {
        public Elevator SelectOptimalElevator(List<Elevator> elevators, Request request)
        {
            Elevator bestElevator = null;
            int minDistance = int.MaxValue;

            foreach (Elevator elevator in elevators)
            {
                int distance = Math.Abs(elevator.CurrentFloor - request.RequestedFloor);
                Direction currentDirection = elevator.CurrentDirection;

                bool isMovingTowards = (currentDirection == Direction.Up && elevator.CurrentFloor <= request.RequestedFloor)
                    || (currentDirection == Direction.Down && elevator.CurrentFloor >= request.RequestedFloor);

                if ((currentDirection == Direction.Idle || isMovingTowards) && distance < minDistance)
                {
                    minDistance = distance;
                    bestElevator = elevator;
                }
            }

            return bestElevator ?? elevators.First();
        }
    }

    // One elevator instance. It owns its own request queue and runs on a separate thread.
    public sealed class Elevator
    {
        private readonly object _lock = new object();
        private readonly SortedSet<int> _upRequests = new SortedSet<int>();
        private readonly SortedSet<int> _downRequests = new SortedSet<int>(Comparer<int>.Create((a, b) => b.CompareTo(a)));

        private int _currentFloor;
        private Direction _currentDirection;

        public Elevator(int id)
        {
            Id = id;
            _currentFloor = 0; // Ground floor
            _currentDirection = Direction.Idle;
        }

        public int Id { get; }

        public int CurrentFloor
        {
            get
            {
                lock (_lock)
                {
                    return _currentFloor;
                }
            }
        }

        public Direction CurrentDirection
        {
            get
            {
                lock (_lock)
                {
                    return _currentDirection;
                }
            }
        }

        // Called when a request is assigned to this elevator.
        public void AddRequest(Request request)
        {
            lock (_lock)
            {
                if (request.Direction == Direction.Up)
                {
                    _upRequests.Add(request.RequestedFloor);
                }
                else
                {
                    _downRequests.Add(request.RequestedFloor);
                }

                if (_currentDirection == Direction.Idle)
                {
                    _currentDirection = request.RequestedFloor > _currentFloor ? Direction.Up : Direction.Down;
                }
            }
        }

        public void Run()
        {
            while (true)
            {
                try
                {
                    ProcessRequests();
                    Thread.Sleep(1000);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Elevator {Id} encountered an error: {ex.Message}");
                    break;
                }
            }
        }

        private void ProcessRequests()
        {
            if (CurrentDirection == Direction.Up || CurrentDirection == Direction.Idle)
            {
                ProcessUpRequests();
                ProcessDownRequests();
            }
            else
            {
                ProcessDownRequests();
                ProcessUpRequests();
            }

            lock (_lock)
            {
                if (_upRequests.Count == 0 && _downRequests.Count == 0)
                {
                    _currentDirection = Direction.Idle;
                }
            }
        }

        private void ProcessUpRequests()
        {
            while (true)
            {
                int nextFloor;
                lock (_lock)
                {
                    if (_upRequests.Count == 0)
                    {
                        break;
                    }

                    nextFloor = _upRequests.Min;
                    _upRequests.Remove(nextFloor);
                }

                MoveToFloor(nextFloor);
            }

            lock (_lock)
            {
                if (_downRequests.Count > 0)
                {
                    _currentDirection = Direction.Down;
                }
            }
        }

        private void ProcessDownRequests()
        {
            while (true)
            {
                int nextFloor;
                lock (_lock)
                {
                    if (_downRequests.Count == 0)
                    {
                        break;
                    }

                    nextFloor = _downRequests.Max;
                    _downRequests.Remove(nextFloor);
                }

                MoveToFloor(nextFloor);
            }

            lock (_lock)
            {
                if (_upRequests.Count > 0)
                {
                    _currentDirection = Direction.Up;
                }
            }
        }

        private void MoveToFloor(int targetFloor)
        {
            int fromFloor;
            lock (_lock)
            {
                fromFloor = _currentFloor;
            }

            Console.WriteLine($"Elevator {Id} moving from {fromFloor} to {targetFloor}");
            Thread.Sleep(Math.Abs(fromFloor - targetFloor) * 500);

            lock (_lock)
            {
                _currentFloor = targetFloor;
            }

            Console.WriteLine($"Elevator {Id} opened doors at floor {targetFloor}");
        }
    }

    // ElevatorController coordinates all elevators and dispatches requests.
    public sealed class ElevatorController
    {
        private readonly List<Elevator> _elevators;
        private DispatchStrategy _dispatchStrategy;

        public ElevatorController(int numElevators, DispatchStrategy strategy)
        {
            _elevators = new List<Elevator>();
            _dispatchStrategy = strategy;

            for (int i = 1; i <= numElevators; i++)
            {
                Elevator elevator = new Elevator(i);
                _elevators.Add(elevator);

                Thread elevatorThread = new Thread(elevator.Run);
                elevatorThread.IsBackground = true;
                elevatorThread.Start();
            }
        }

        public void SetDispatchStrategy(DispatchStrategy strategy)
        {
            _dispatchStrategy = strategy;
        }

        // This is called when a user presses a button in the hallway.
        public void SubmitExternalRequest(int floor, Direction direction)
        {
            Request request = new Request(floor, direction);
            Elevator optimalElevator = _dispatchStrategy.SelectOptimalElevator(_elevators, request);

            Console.WriteLine($"Dispatcher assigned Elevator {optimalElevator.Id} to floor {floor}");
            optimalElevator.AddRequest(request);
        }

        // This is called when a user presses a button inside an elevator.
        public void SubmitInternalRequest(int elevatorId, int targetFloor)
        {
            Elevator elevator = _elevators.FirstOrDefault(e => e.Id == elevatorId);

            if (elevator == null)
            {
                Console.Error.WriteLine($"Error: Elevator with ID {elevatorId} not found.");
                return;
            }

            Direction direction = targetFloor > elevator.CurrentFloor ? Direction.Up : Direction.Down;
            elevator.AddRequest(new Request(targetFloor, direction));
        }
    }

    public static class ElevatorSystemDemo
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("--- Elevator System Booted ---");

            ElevatorController controller = new ElevatorController(3, new NearestElevatorStrategy());

            // Simulate hallway requests.
            // Request 1: nearest elevator is Elevator 1, because it is idle at floor 0.
            controller.SubmitExternalRequest(5, Direction.Up);

            // Add a small delay so the elevator can begin moving.
            Thread.Sleep(1500);

            // Request 2: now Elevator 2 is closer to the floor 8 request than Elevator 1,
            // so the dispatcher should choose a different elevator.
            controller.SubmitExternalRequest(8, Direction.Up);

            // Request 3: a lower-floor request from another direction should also choose the
            // elevator that is already moving closer, not always Elevator 1.
            controller.SubmitExternalRequest(2, Direction.Down);

            Thread.Sleep(1000);

            // Simulate someone entering Elevator 1 and pressing floor 10.
            controller.SubmitInternalRequest(1, 10);

            // Simulate a request for a different elevator so the demo shows multiple
            // elevators being used in the same system.
            controller.SubmitInternalRequest(2, 12);

            Thread.Sleep(10000);
        }
    }
}
