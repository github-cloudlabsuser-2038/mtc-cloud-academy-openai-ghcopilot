// Generated with EchoBot .NET Template version v4.22.0

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Bot.Builder;
using Microsoft.Bot.Schema;

namespace EchoBot.Bots
{
    public class EchoBot : ActivityHandler
    {
    //"Generar 2 propiedades para la llave y el endpoint de Azure OpenAI, con un valor predeterminado

    public string AzureOpenAIKey { get; set; } = "2650f03a3fc1482691ba6197b3e856a4";
    public string AzureOpenAIEndpoint { get; set; } = "https://aoai-dev-asp-aoaighcopilotws.openai.azure.com/";

    //Generar un método asíncrono que regrese un string y que reciba como parámetro un texto, el método esta vacío
    public async Task<string> GetResponseAsync(string text)
    {
    //generar un AzureKeyCredential con la llave de OpenAI
    var credential = new AzureKeyCredential(AzureOpenAIKey);
    //inicializar el cliente de OpenAIClient con el endpoint de OpenAI y el AzureKeyCredential
    var client = new OpenAIClient(AzureOpenAIEndpoint, credential);
    //"Inicializar las opciones de ChatCompletionsOptions de forma inline indicando el parámetro de DeploymentName de tipo gpt-35-turbo y Messages recibido de la variable text de tipo ChatRequestUserMessage. Siguiendo el formato new Class { Property = Value }"
    var options = new ChatCompletionsOptions
    {
        DeploymentName = "gpt-35-turbo",
        Messages = new ChatRequestUserMessage[] { new ChatRequestUserMessage { Role = "user", Content = text } }
    };
    //Invocar el método GetChatCompletionsAsync y regresar la respuesta de la llamada
    var response = await client.GetChatCompletionsAsync(options);

    //regresar el texto de la respuesta de Choices

    return response.ChatCompletion[0].Message.Content;

    }

       
    // El resto del código...
}
        protected override async Task OnMessageActivityAsync(ITurnContext<IMessageActivity> turnContext, CancellationToken cancellationToken)
        {
            //    //"Declarar una variable de tipo string llamada replyText y asignarle el valor de la respuesta de GetOpenAIResponseAsync con el parámetro de texto de la actividad recibida"
            var replyText = await GetResponseAsync(turnContext.Activity.Text);
            await turnContext.SendActivityAsync(MessageFactory.Text(replyText, replyText), cancellationToken);
        }

        protected override async Task OnMembersAddedAsync(IList<ChannelAccount> membersAdded, ITurnContext<IConversationUpdateActivity> turnContext, CancellationToken cancellationToken)
        {
            var welcomeText = "Hello and welcome!";
            foreach (var member in membersAdded)
            {
                if (member.Id != turnContext.Activity.Recipient.Id)
                {
                    await turnContext.SendActivityAsync(MessageFactory.Text(welcomeText, welcomeText), cancellationToken);
                }
            }
        }
    }
}
