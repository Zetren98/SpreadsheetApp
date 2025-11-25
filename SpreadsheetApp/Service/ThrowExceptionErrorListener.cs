using Antlr4.Runtime;

namespace SpreadsheetApp.Services
{
    public class ThrowExceptionErrorListener : BaseErrorListener
    {
        public override void SyntaxError(IRecognizer recognizer, IToken offendingSymbol, int line, int charPositionInLine, string msg, RecognitionException e)
        {
            throw new Exception($"Помилка синтаксису: {msg}");
        }
    }
}