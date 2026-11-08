namespace ModernRecrut.MVC.Services
{
    public class JournalError
    {

        public static void AppliquerJournal(
            ILogger logger,
            HttpResponseMessage response,
            string nomService)
        {
            int code = (int)response.StatusCode;

            string message =
                $"Erreur HTTP {code} lors de l'appel au service {nomService}. " +
                $"URL : {response.RequestMessage?.RequestUri}";

            if (code >= 400 && code <= 499)
            {
                logger.LogError(
                    "Code HTTP {StatusCode} - {Message}",
                    code,
                    message
                );
            }
            else if (code >= 500 && code <= 599)
            {
                logger.LogCritical(
                    "Code HTTP {StatusCode} - {Message}",
                    code,
                    message
                );
            }
        }
    }
}
