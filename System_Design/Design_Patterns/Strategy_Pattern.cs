//Strategy Interface
public interface IWeaponStrategy
{
    void UseWeapon();
}

//Concreete classes implement strategy
public class SwordStrategy: IWeaponStrategy
{
    public void UseWeapon()=> System.Console.WriteLine("slashed");
}
public class BowStrategy : IWeaponStrategy
{
    public void UseWeapon()=> System.Console.WriteLine("Shot an arrow");
}

// Context class using the strategies lets it change at runtime
public class GameCharacter
{
    // A slot to hold whatever weapon the character currently has equipped
    private IWeaponStrategy _currentWeapon;
    //when character is born giving them a weapon
    public GameCharacter(IWeaponStrategy startingWeapon)
    {
        _currentWeapon = startingWeapon;
    }
    //changing the weapon
    public void ChangeWeapon(IWeaponStrategy newWeapon)
    {
        _currentWeapon = newWeapon;
    }
    //when player does attack
    public void PressAttackButton()
    {
        _currentWeapon.UseWeapon();
    }
}
class Program
{
    static void Main()
    {
        // 1. Create the character (Context) and start them with a Sword
        GameCharacter hero = new GameCharacter(new SwordStrategy());

        // 2. Press attack
        hero.PressAttackButton(); // Output: Slash! Swung the sword!

        // 3. The player picks up a bow. We swap the strategy dynamically!
        hero.ChangeWeapon(new BowStrategy());

        // 4. Press attack again
        hero.PressAttackButton(); // Output: Thwip! Shot an arrow!
    }
}