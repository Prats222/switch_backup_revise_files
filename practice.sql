/*
SQL SERVER PRACTICE WORKBOOK
Purpose: interview practice + long-term revision
Dialect: Microsoft SQL Server / T-SQL

HOW TO USE
1. Run Section 0 once in a fresh SQL Server database.
2. Work through each solved example.
3. Immediately attempt the matching PRACTICE question without looking for a solution.
4. The practice questions intentionally have NO solutions.
5. Re-run Section 0 to reset the sample data if you modify it.

Note: This script does not create/select a database. Run it in a database where
you have permission to create tables. If these tables already exist, drop them
first or use a fresh practice database.
*/

-- SET NOCOUNT ON;
-- GO

/* ============================================================
   SECTION 0 — TABLES, KEYS, CONSTRAINTS, AND SEEDED DATA
   ============================================================ */

DROP TABLE IF EXISTS OrderItems;
DROP TABLE IF EXISTS Orders;
DROP TABLE IF EXISTS Employees;
DROP TABLE IF EXISTS Customers;
DROP TABLE IF EXISTS Products;
DROP TABLE IF EXISTS Departments;
GO

CREATE TABLE Departments (
    DepartmentId INT NOT NULL PRIMARY KEY,
    DepartmentName VARCHAR(50) NOT NULL UNIQUE,
    Location VARCHAR(50) NULL
);

CREATE TABLE Employees (
    EmployeeId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    EmployeeName VARCHAR(100) NOT NULL,
    Email VARCHAR(150) NOT NULL UNIQUE,
    DepartmentId INT NULL,
    ManagerId INT NULL,
    Salary DECIMAL(10,2) NOT NULL CHECK (Salary >= 0),
    JoiningDate DATE NOT NULL,
    IsActive BIT NOT NULL DEFAULT 1,
    CONSTRAINT FK_Employees_Departments FOREIGN KEY (DepartmentId)
        REFERENCES Departments(DepartmentId),
    CONSTRAINT FK_Employees_Manager FOREIGN KEY (ManagerId)
        REFERENCES Employees(EmployeeId)
);

CREATE TABLE Customers (
    CustomerId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CustomerName VARCHAR(100) NOT NULL,
    Email VARCHAR(150) NULL UNIQUE,
    City VARCHAR(60) NULL,
    SignupDate DATE NOT NULL
);

CREATE TABLE Products (
    ProductId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    ProductName VARCHAR(100) NOT NULL,
    Category VARCHAR(50) NOT NULL,
    Price DECIMAL(10,2) NOT NULL CHECK (Price >= 0),
    Stock INT NOT NULL CHECK (Stock >= 0)
);

CREATE TABLE Orders (
    OrderId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    CustomerId INT NOT NULL,
    OrderDate DATE NOT NULL,
    Status VARCHAR(20) NOT NULL DEFAULT 'Pending',
    CONSTRAINT FK_Orders_Customers FOREIGN KEY (CustomerId)
        REFERENCES Customers(CustomerId),
    CONSTRAINT CK_Orders_Status CHECK (Status IN ('Pending','Shipped','Delivered','Cancelled'))
);

CREATE TABLE OrderItems (
    OrderItemId INT IDENTITY(1,1) NOT NULL PRIMARY KEY,
    OrderId INT NOT NULL,
    ProductId INT NOT NULL,
    Quantity INT NOT NULL CHECK (Quantity > 0),
    UnitPrice DECIMAL(10,2) NOT NULL CHECK (UnitPrice >= 0),
    CONSTRAINT FK_OrderItems_Orders FOREIGN KEY (OrderId)
        REFERENCES Orders(OrderId),
    CONSTRAINT FK_OrderItems_Products FOREIGN KEY (ProductId)
        REFERENCES Products(ProductId),
    CONSTRAINT UQ_OrderItems_Order_Product UNIQUE (OrderId, ProductId)
);
GO

INSERT INTO Departments (DepartmentId, DepartmentName, Location) VALUES
(10, 'IT', 'Chennai'),
(20, 'HR', 'Bengaluru'),
(30, 'Finance', 'Mumbai'),
(40, 'Sales', 'Hyderabad'),
(50, 'Legal', 'Delhi');

INSERT INTO Employees
(EmployeeName, Email, DepartmentId, ManagerId, Salary, JoiningDate, IsActive)
VALUES
('Aman',    'aman@example.com',    10, NULL, 90000, '2020-01-15', 1),
('Prateek', 'prateek@example.com', 10, 1,    80000, '2021-06-10', 1),
('Neha',    'neha@example.com',    10, 1,    80000, '2022-03-20', 1),
('Rahul',   'rahul@example.com',   20, NULL, 60000, '2019-11-05', 1),
('Priya',   'priya@example.com',   20, 4,    55000, '2023-02-12', 1),
('Vikram',  'vikram@example.com',  30, NULL, 70000, '2020-08-01', 1),
('Sara',    'sara@example.com',    40, NULL, 50000, '2024-01-08', 1),
('Karan',   'karan@example.com',   40, 7,    45000, '2024-05-15', 0),
('Meera',   'meera@example.com',   NULL, NULL, 40000, '2025-01-01', 1),
('Ishaan',  'ishaan@example.com',  10, 1,    65000, '2023-07-07', 1);

INSERT INTO Customers (CustomerName, Email, City, SignupDate) VALUES
('Ananya', 'ananya@example.com', 'Chennai', '2023-01-10'),
('Bharat', 'bharat@example.com', 'Bengaluru', '2023-04-22'),
('Charu',  'charu@example.com',  'Mumbai', '2024-02-15'),
('Deepak', 'deepak@example.com', 'Chennai', '2024-06-01'),
('Esha',   'esha@example.com',   NULL, '2025-01-20'),
('Farhan', 'farhan@example.com', 'Delhi', '2025-03-10'),
('Gita',   NULL,                 'Hyderabad', '2025-04-05');

INSERT INTO Products (ProductName, Category, Price, Stock) VALUES
('Laptop', 'Electronics', 65000, 12),
('Mouse', 'Electronics', 800, 100),
('Keyboard', 'Electronics', 1800, 60),
('Office Chair', 'Furniture', 7500, 20),
('Desk', 'Furniture', 12000, 8),
('Notebook', 'Stationery', 100, 500),
('Pen Pack', 'Stationery', 250, 250),
('Monitor', 'Electronics', 15000, 15);

INSERT INTO Orders (CustomerId, OrderDate, Status) VALUES
(1, '2024-01-10', 'Delivered'),
(1, '2024-02-15', 'Delivered'),
(2, '2024-03-02', 'Shipped'),
(3, '2024-03-10', 'Cancelled'),
(4, '2024-04-20', 'Delivered'),
(2, '2024-05-01', 'Pending'),
(6, '2025-02-11', 'Delivered'),
(1, '2025-03-05', 'Shipped');

INSERT INTO OrderItems (OrderId, ProductId, Quantity, UnitPrice) VALUES
(1, 1, 1, 65000),
(1, 2, 2, 800),
(2, 3, 1, 1800),
(2, 6, 10, 100),
(3, 4, 2, 7500),
(3, 2, 1, 800),
(4, 8, 1, 15000),
(5, 5, 1, 12000),
(5, 6, 20, 100),
(6, 1, 1, 64000),
(7, 8, 2, 15000),
(8, 4, 1, 7500);
GO

-- /* ============================================================
--    SECTION 1 — BASIC SELECT, FILTERING, SORTING, DISTINCT
--    ============================================================ */

-- -- Q01 SOLVED: Show employee names and salaries.
SELECT EmployeeName, Salary
FROM Employees;

-- -- Q01 PRACTICE: Show product names and prices.
select ProductName , Price from Products;

-- Q02 SOLVED: List employees earning more than 60,000.
select EmployeeName, Salary from Employees where salary>60000
-- Q02 PRACTICE: List employees earning less than 55,000.
select EmployeeName , Salary as Data from Employees where Salary<55000
-- Q03 SOLVED: List employees from highest salary to lowest.
select EmployeeName,Salary from Employees order by Salary desc , EmployeeName asc;

-- Q03 PRACTICE: List employees from earliest joining date to latest.
select EmployeeName , JoiningDate from Employees order by JoiningDate asc;
-- Q04 SOLVED: Find employees in the IT department.
SELECT e.EmployeeName
FROM Employees e
JOIN Departments d ON d.DepartmentId = e.DepartmentId
WHERE d.DepartmentName = 'IT';

-- Q04 PRACTICE: Find employees in the HR department.
select e.EmployeeName , d.DepartmentName 
from Employees e 
join Departments d on d.DepartmentId = e.DepartmentId 
where d.DepartmentName = 'HR' 
-- Q05 SOLVED: Return distinct customer cities.
SELECT DISTINCT City
FROM Customers
WHERE City IS NOT NULL
ORDER BY City;

-- Q05 PRACTICE: Return distinct product categories.
select distinct Category as uniqueCategory from Products where Category is NOT NULL
-- Q06 SOLVED: Find employees whose salary is between 50,000 and 80,000 inclusive.
SELECT EmployeeName, Salary
FROM Employees
WHERE Salary BETWEEN 50000 AND 80000;

-- Q06 PRACTICE: Find products priced between 1,000 and 10,000 inclusive.
select ProductName , Price from Products where Price between 1000 and 10000
-- Q07 SOLVED: Find customers from Chennai or Mumbai.
SELECT CustomerName, City
FROM Customers
WHERE City IN ('Chennai', 'Mumbai');

-- Q07 PRACTICE: Find employees belonging to department IDs 10 or 40.
select EmployeeName, DepartmentId from Employees where DepartmentId in(10,40)
-- Q08 SOLVED: Find employees whose names start with 'P'.
SELECT EmployeeName
FROM Employees
WHERE EmployeeName LIKE 'P%';

-- Q08 PRACTICE: Find customers whose names end with 'a'.
select CustomerName from Customers where CustomerName like '%a'

-- Q09 SOLVED: Return the five highest-paid employees (SQL Server).
select top(5) EmployeeName, Salary from Employees order by salary desc
-- Q09 PRACTICE: Return the three cheapest products.
select top(3) ProductName , Price from Products order by Price asc
-- Q10 SOLVED: Find employees without a department.
SELECT EmployeeName
FROM Employees
WHERE DepartmentId IS NULL;

-- Q10 PRACTICE: Find customers whose city is unknown.
select CustomerName, City from Customers where city is NULL
/* ============================================================
   SECTION 2 — INSERT, UPDATE, DELETE, ALTER, CASE
   ============================================================ */

-- Q11 SOLVED: Add a new product.
INSERT INTO Products (ProductName, Category, Price, Stock)
VALUES ('USB Cable', 'Electronics', 400, 75);

-- Q11 PRACTICE: Insert a new product of your choice. Use a unique name.

-- Q12 SOLVED: Increase the price of Mouse by 10%.
UPDATE Products
SET Price = Price * 1.10
WHERE ProductName = 'Mouse';

-- Q12 PRACTICE: Increase the price of Notebook by 5%.

-- Q13 SOLVED: Use CASE to label employees by salary.
SELECT EmployeeName, Salary,
       CASE
           WHEN Salary >= 80000 THEN 'High'
           WHEN Salary >= 60000 THEN 'Medium'
           ELSE 'Entry'
       END AS SalaryBand
FROM Employees;

-- Q13 PRACTICE: Label products as 'Expensive' when Price >= 10000, otherwise 'Affordable'.

-- Q14 SOLVED: Add a nullable column to a table.
ALTER TABLE Customers ADD Phone VARCHAR(20) NULL;

-- Q14 PRACTICE: Add a nullable column named Notes (VARCHAR(200)) to Products.

-- Q15 SOLVED: Safely preview rows that would be updated.
SELECT ProductId, ProductName, Stock
FROM Products
WHERE Stock < 10;

-- Q15 PRACTICE: Write a SELECT that previews employees whose IsActive value is 0.

-- Q16 SOLVED: Demonstrate a transaction that rolls back an update.
BEGIN TRANSACTION;
UPDATE Products SET Stock = Stock - 1 WHERE ProductName = 'Laptop';
SELECT ProductName, Stock FROM Products WHERE ProductName = 'Laptop';
ROLLBACK TRANSACTION;

-- Q16 PRACTICE: In a transaction, temporarily change the price of Desk and then ROLLBACK.

/* ============================================================
   SECTION 3 — AGGREGATES, GROUP BY, HAVING
   ============================================================ */

-- Q17 SOLVED: Count all employees.
SELECT COUNT(*) AS EmployeeCount
FROM Employees;

-- Q17 PRACTICE: Count all customers.
select count(*) as custCount from Customers

-- Q18 SOLVED: Count employees in each department, including employees with no department.
SELECT COALESCE(d.DepartmentName, 'No Department') AS Department,
       COUNT(*) AS EmployeeCount
FROM Employees e
LEFT JOIN Departments d ON d.DepartmentId = e.DepartmentId
GROUP BY d.DepartmentName
ORDER BY EmployeeCount DESC;

-- Q18 PRACTICE: Count products in each category.
select coalesce(p.Category,'No category found') as Category,
count(*) as productCount
from Products p
group by p.Category
order by productCount desc;

-- Q19 SOLVED: Calculate average salary per department.
SELECT d.DepartmentName, AVG(e.Salary) AS AverageSalary
FROM Departments d
JOIN Employees e ON e.DepartmentId = d.DepartmentId
GROUP BY d.DepartmentName;

-- Q19 PRACTICE: Calculate the average product price per category.
select avg(price) as avgPrice, Category from Products group by Category

-- Q20 SOLVED: Show departments with at least three employees.
SELECT d.DepartmentName, COUNT(*) AS EmployeeCount
FROM Departments d
JOIN Employees e ON e.DepartmentId = d.DepartmentId
GROUP BY d.DepartmentName
HAVING COUNT(*) >= 3;

-- Q20 PRACTICE: Show product categories containing at least three products.
select category, count(*) as ProdCount from Products group by Category having count(*)>=3

-- Q21 SOLVED: Show departments whose average salary exceeds 60,000.
SELECT d.DepartmentName, AVG(e.Salary) AS AverageSalary
FROM Departments d
JOIN Employees e ON e.DepartmentId = d.DepartmentId
GROUP BY d.DepartmentName
HAVING AVG(e.Salary) > 60000;

-- Q21 PRACTICE: Show categories whose average product price exceeds 5,000.
select category , avg(price) as productPrice from Products group by Category having avg(price)>5000

-- Q22 SOLVED: Count delivered orders.
SELECT COUNT(*) AS DeliveredOrders
FROM Orders
WHERE Status = 'Delivered';

-- Q22 PRACTICE: Count cancelled orders.
SELECT COUNT(*) AS UnDeliveredOrders
FROM Orders
WHERE Status != 'Delivered';

-- Q23 SOLVED: Find the minimum and maximum salary.
SELECT MIN(Salary) AS MinimumSalary, MAX(Salary) AS MaximumSalary
FROM Employees;

-- Q23 PRACTICE: Find the cheapest and most expensive product prices.
select min(Price) as MinPrice , Max(Price) from Products 

-- Q24 SOLVED: Total order-item revenue by product.
SELECT p.ProductName, SUM(oi.Quantity * oi.UnitPrice) AS Revenue
FROM Products p
JOIN OrderItems oi ON oi.ProductId = p.ProductId
GROUP BY p.ProductId, p.ProductName
ORDER BY Revenue DESC;

-- Q24 PRACTICE: Calculate total revenue for each order.
SELECT o.OrderId, COALESCE(SUM(oi.Quantity * oi.UnitPrice), 0) AS TotalRevenue
FROM Orders o
LEFT JOIN OrderItems oi ON oi.OrderId = o.OrderId
GROUP BY o.OrderId
ORDER BY o.OrderId;

/* ============================================================
   SECTION 4 — JOINS
   ============================================================ */

-- Q25 SOLVED: Show employee names with their department names.
SELECT e.EmployeeName, d.DepartmentName
FROM Employees e
INNER JOIN Departments d ON d.DepartmentId = e.DepartmentId;

-- Q25 PRACTICE: Show each order ID with the name of the customer who placed it.

-- Q26 SOLVED: List all departments, even those with no employees.
SELECT d.DepartmentName, e.EmployeeName
FROM Departments d
LEFT JOIN Employees e ON e.DepartmentId = d.DepartmentId
ORDER BY d.DepartmentName, e.EmployeeName;

-- Q26 PRACTICE: List all customers and any orders they have placed, including customers with no orders.

-- Q27 SOLVED: Find departments with no employees.
SELECT d.DepartmentName
FROM Departments d
LEFT JOIN Employees e ON e.DepartmentId = d.DepartmentId
WHERE e.EmployeeId IS NULL;

-- Q27 PRACTICE: Find customers who have never placed an order.

-- Q28 SOLVED: Show employee-manager pairs (self join).
SELECT e.EmployeeName AS Employee, m.EmployeeName AS Manager
FROM Employees e
LEFT JOIN Employees m ON m.EmployeeId = e.ManagerId;

-- Q28 PRACTICE: Show each employee with their manager, but return only employees who have a manager.

-- Q29 SOLVED: Show order IDs, customer names, product names and quantities.
SELECT o.OrderId, c.CustomerName, p.ProductName, oi.Quantity
FROM Orders o
JOIN Customers c ON c.CustomerId = o.CustomerId
JOIN OrderItems oi ON oi.OrderId = o.OrderId
JOIN Products p ON p.ProductId = oi.ProductId
ORDER BY o.OrderId;

-- Q29 PRACTICE: Show order ID, order date, customer name and order status.

-- Q30 SOLVED: Find customers with at least one delivered order.
SELECT DISTINCT c.CustomerId, c.CustomerName
FROM Customers c
JOIN Orders o ON o.CustomerId = c.CustomerId
WHERE o.Status = 'Delivered';

-- Q30 PRACTICE: Find customers with at least one cancelled order.

-- Q31 SOLVED: Calculate total value per order.
SELECT o.OrderId, SUM(oi.Quantity * oi.UnitPrice) AS OrderTotal
FROM Orders o
JOIN OrderItems oi ON oi.OrderId = o.OrderId
GROUP BY o.OrderId
ORDER BY o.OrderId;

-- Q31 PRACTICE: Calculate total quantity of items per order.

/* ============================================================
   SECTION 5 — SUBQUERIES, IN, EXISTS
   ============================================================ */

-- Q32 SOLVED: Employees earning above the overall average salary.
SELECT EmployeeName, Salary
FROM Employees
WHERE Salary > (SELECT AVG(Salary) FROM Employees);

-- Q32 PRACTICE: Products priced above the overall average product price.

-- Q33 SOLVED: Find employees who work in departments located in Chennai.
SELECT EmployeeName
FROM Employees
WHERE DepartmentId IN (
    SELECT DepartmentId
    FROM Departments
    WHERE Location = 'Chennai'
);

-- Q33 PRACTICE: Find employees who work in departments located in Bengaluru.

-- Q34 SOLVED: Find customers who have at least one order using EXISTS.
SELECT c.CustomerName
FROM Customers c
WHERE EXISTS (
    SELECT 1
    FROM Orders o
    WHERE o.CustomerId = c.CustomerId
);

-- Q34 PRACTICE: Find customers who have no orders using NOT EXISTS.

-- Q35 SOLVED: Find the highest-paid employee(s).
SELECT EmployeeName, Salary
FROM Employees
WHERE Salary = (SELECT MAX(Salary) FROM Employees);

-- Q35 PRACTICE: Find the cheapest product(s).

-- Q36 SOLVED: Find products that have never been ordered.
SELECT p.ProductName
FROM Products p
WHERE NOT EXISTS (
    SELECT 1
    FROM OrderItems oi
    WHERE oi.ProductId = p.ProductId
);

-- Q36 PRACTICE: Find customers who have never had a delivered order.

-- Q37 SOLVED: Find employees earning more than the average salary in their own department.
SELECT e.EmployeeName, e.Salary, e.DepartmentId
FROM Employees e
WHERE e.DepartmentId IS NOT NULL
  AND e.Salary > (
      SELECT AVG(e2.Salary)
      FROM Employees e2
      WHERE e2.DepartmentId = e.DepartmentId
  );

-- Q37 PRACTICE: Find products priced above the average price of their own category.

/* ============================================================
   SECTION 6 — CTEs AND WINDOW FUNCTIONS
   ============================================================ */

-- Q38 SOLVED: Use a CTE to find employees earning above average.
WITH SalaryStats AS (
    SELECT AVG(Salary) AS AverageSalary
    FROM Employees
)
SELECT e.EmployeeName, e.Salary
FROM Employees e
CROSS JOIN SalaryStats s
WHERE e.Salary > s.AverageSalary;

-- Q38 PRACTICE: Use a CTE to calculate average price and return products above it.

-- Q39 SOLVED: Number employees within each department by salary.
SELECT EmployeeName, DepartmentId, Salary,
       ROW_NUMBER() OVER (
           PARTITION BY DepartmentId
           ORDER BY Salary DESC, EmployeeId
       ) AS RowNum
FROM Employees;

-- Q39 PRACTICE: Number products within each category from most expensive to cheapest.

-- Q40 SOLVED: Rank employees by salary, allowing ties and gaps.
SELECT EmployeeName, Salary,
       RANK() OVER (ORDER BY Salary DESC) AS SalaryRank
FROM Employees;

-- Q40 PRACTICE: Rank products by price using RANK().

-- Q41 SOLVED: Dense-rank employees by salary, with no gaps between ranks.
SELECT EmployeeName, Salary,
       DENSE_RANK() OVER (ORDER BY Salary DESC) AS SalaryDenseRank
FROM Employees;

-- Q41 PRACTICE: Use DENSE_RANK() to rank products by price.

-- Q42 SOLVED: Return the second-highest distinct salary.
WITH RankedSalaries AS (
    SELECT EmployeeName, Salary,
           DENSE_RANK() OVER (ORDER BY Salary DESC) AS SalaryRank
    FROM Employees
)
SELECT EmployeeName, Salary
FROM RankedSalaries
WHERE SalaryRank = 2;

-- Q42 PRACTICE: Return all employees earning the third-highest distinct salary.

-- Q43 SOLVED: Return the highest-paid employee in each department.
WITH RankedEmployees AS (
    SELECT EmployeeName, DepartmentId, Salary,
           ROW_NUMBER() OVER (
               PARTITION BY DepartmentId
               ORDER BY Salary DESC, EmployeeId
           ) AS rn
    FROM Employees
    WHERE DepartmentId IS NOT NULL
)
SELECT EmployeeName, DepartmentId, Salary
FROM RankedEmployees
WHERE rn = 1;

-- Q43 PRACTICE: Return the two highest-paid employees in each department.

-- Q44 SOLVED: Calculate a running total of order revenue by date.
WITH DailyOrderTotals AS (
    SELECT o.OrderDate, o.OrderId,
           SUM(oi.Quantity * oi.UnitPrice) AS OrderTotal
    FROM Orders o
    JOIN OrderItems oi ON oi.OrderId = o.OrderId
    GROUP BY o.OrderDate, o.OrderId
)
SELECT OrderDate, OrderId, OrderTotal,
       SUM(OrderTotal) OVER (
           ORDER BY OrderDate, OrderId
           ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW
       ) AS RunningRevenue
FROM DailyOrderTotals;

-- Q44 PRACTICE: Calculate a running total of employee salaries ordered by JoiningDate.

-- Q45 SOLVED: Compare each employee salary with the previous salary within their department.
SELECT EmployeeName, DepartmentId, Salary,
       LAG(Salary) OVER (
           PARTITION BY DepartmentId
           ORDER BY JoiningDate, EmployeeId
       ) AS PreviousSalary
FROM Employees;

-- Q45 PRACTICE: Use LEAD() to show the next employee's joining date within each department.

/* ============================================================
   SECTION 9 — 20 ADDITIONAL INTERVIEW CHALLENGES (NO SOLUTIONS)
   Try these only after finishing the paired practice prompts.
   ============================================================ */

-- CHALLENGE 01: Find the third-highest distinct salary without using TOP.
-- CHALLENGE 02: Find employees who earn the same salary as at least one other employee.
-- CHALLENGE 03: Find the department with the highest average salary.
-- CHALLENGE 04: Find the department with the largest number of employees.
-- CHALLENGE 05: Find customers who have placed orders in more than one calendar month.
-- CHALLENGE 06: Find the best-selling product by total quantity sold.
-- CHALLENGE 07: Find the customer with the highest total spend.
-- CHALLENGE 08: Find products that have never been ordered using LEFT JOIN.
-- CHALLENGE 09: Find employees whose salary is greater than their manager's salary.
-- CHALLENGE 10: Find the latest order for each customer.
-- CHALLENGE 11: Return the top 2 products by revenue in each category.
-- CHALLENGE 12: Find consecutive order dates if any exist (use LAG or date logic).
-- CHALLENGE 13: Show each employee's salary and difference from department average.
-- CHALLENGE 14: Find customers who ordered every product in the Electronics category.
-- CHALLENGE 15: Find the month with the highest order revenue.
-- CHALLENGE 16: Find orders that contain more than one distinct product.
-- CHALLENGE 17: Find customers who have only cancelled orders.
-- CHALLENGE 18: Find each department's salary total and percentage of company payroll.
-- CHALLENGE 19: Safely delete duplicate records in a hypothetical table while retaining one row.
-- CHALLENGE 20: Explain how you would investigate and improve a slow query using an execution plan.

 /*
END OF WORKBOOK

Recommended workflow:
- Complete each paired PRACTICE prompt immediately after its SOLVED example.
- Then attempt the 20 challenge questions.
- Do not run all practice prompts as one batch: each is a task to solve.
*/
