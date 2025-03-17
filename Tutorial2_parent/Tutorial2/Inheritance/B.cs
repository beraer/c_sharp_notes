namespace Tutorial2.Inheritance;

public class B : A
{
    public int AdditionalValue { get; set; }

    public B(int additionalValue) 
    {
        AdditionalValue = additionalValue;
    }
}