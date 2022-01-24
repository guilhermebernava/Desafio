namespace qodeless.desafio.webapi
{
    public class JWTSetup
    {
        public string Secret { get; set; }

        public int Duration { get; set; }

        public string Emiter { get; set; }

        public string ValidOn { get; set; }
    }
}
