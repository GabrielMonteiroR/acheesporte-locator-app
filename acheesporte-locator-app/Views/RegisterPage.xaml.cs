using acheesporte_locator_app.ViewModels;

namespace acheesporte_locator_app.Views;

public partial class RegisterPage : ContentPage
{
    public RegisterPage()
    {
        InitializeComponent();
        BindingContext = App.Services.GetService<RegisterViewModel>();

        StartTypingAnimation();
    }

    private async void StartTypingAnimation()
    {
        var prefix = "Bem-vindo, novo ";
        var phrases = new List<string> { "Empresário", "Chefe", "Vencedor" };
        var colors = new List<Color> { Colors.DeepPink, Colors.MediumSeaGreen, Colors.DeepSkyBlue };

        int index = 0;

        while (true)
        {
            string phrase = phrases[index];
            Color phraseColor = colors[index];

            for (int i = 1; i <= phrase.Length; i++)
            {
                SetFormattedText(prefix, phrase[..i], phraseColor);
                await Task.Delay(100);
            }

            await Task.Delay(1000);

            for (int i = phrase.Length - 1; i >= 0; i--)
            {
                SetFormattedText(prefix, phrase[..i], phraseColor);
                await Task.Delay(60);
            }

            index = (index + 1) % phrases.Count;
        }
    }

    private void SetFormattedText(string prefix, string dynamicText, Color color)
    {
        TypingLabel.FormattedText = new FormattedString
        {
            Spans =
            {
                new Span
                {
                    Text = prefix,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 24,
                    TextColor = Colors.White
                },
                new Span
                {
                    Text = dynamicText,
                    FontAttributes = FontAttributes.Bold,
                    FontSize = 24,
                    TextColor = color
                }
            }
        };
    }
}
