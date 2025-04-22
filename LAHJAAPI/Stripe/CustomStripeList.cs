namespace LAHJAAPI.Dto;

public class CustomStripeList<T> : IEnumerable<T>
{
    public string Object { get; set; }

    public List<T> Data { get; set; }


    public bool HasMore { get; set; }

    public string Url { get; set; }

    public IEnumerator<T> GetEnumerator()
    {
        return Data.GetEnumerator();
    }

    //
    // Summary:
    //     Reverse the order of the items in Data to support backward iteration in autopagination
    //     with EndingBefore.
    public void Reverse()
    {
        Data.Reverse();
    }

    System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
    {
        return Data.GetEnumerator();
    }
}
