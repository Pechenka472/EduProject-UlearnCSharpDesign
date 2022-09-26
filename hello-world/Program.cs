internal static class Program
{
    public static void Main()
    {
        Console.WriteLine("Start");
        var a = new A
        {
            Number = 1
        };
    }
}
class A
{
    private int _number;
    public int Number
    {
        get => _number;
        set
        {
            Console.WriteLine("Hello, world!");
            _number = value;
        }
    }
}