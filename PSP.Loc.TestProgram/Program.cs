namespace PSP.Loc.TestProgram
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var sourceCode = "dslfjksd { fksdlfjsdlk";
            var i = 0;
            if(i == 3)
            {
                i++; // bla bla { bla
            } else if('\'' == '\\')
            {
                do
                {
                    i++;
                } while (sourceCode[i] != '{');
                loc++; //we've reached a { so we count it //we've reached a { so we count it
            }
        }
    }
}
