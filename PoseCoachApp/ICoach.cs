using System.Drawing.Imaging;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;

namespace PoseCoachApp;

public interface ICoach
{
    Task<string> GetPoseEvaluation(Bitmap image);
}

public class ChatGPTCoach(IChatCompletionService 
    chatCompletionService) : ICoach
{
    
    public async Task<string> GetPoseEvaluation(Bitmap image)
    {
        ChatHistory chatHistory = new();
        chatHistory.AddSystemMessage("""
                                     You are an pose instructor and you are going to evaluate the pose of the person in the image.
                                     Don't comment on the identity of the person in the image, you are strictly focusing on the pose.
                                      The goal is to have good posture and alignment. 
                                      Provide a score from 1 to 10, with 10 being the best possible score together with your evaluation and recommandations.
                                     """);
        chatHistory.AddUserMessage([GetImageContent(image)]);
        var response = await chatCompletionService.GetChatMessageContentsAsync(chatHistory);
        return response.First().Content;
    }
    
    private ImageContent GetImageContent(Bitmap image)
    {
        using var memoryStream = new MemoryStream();
        image.Save(memoryStream, ImageFormat.Jpeg);
        memoryStream.Position = 0;
        
        var imageContent = new ImageContent(memoryStream.ToArray(),"image/jpeg" );
        return imageContent;
    }
}