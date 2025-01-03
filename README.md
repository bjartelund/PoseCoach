#PoseCoach

![bilde](https://github.com/user-attachments/assets/9dd52a31-827f-40ac-924c-ba1c3cbaaa11)

I spend most of my working day by the computer. 
I have a nice chair and a motorized desk, but this does not help if I don't control my posture.

The PoseCoach is a simple Windows Forms application that takes timed images using the webcam(the first one detected) and sends it off to OpenAIs multi-modal LLM.
For now the prompt is super simple, and no attempts is made to cover the face. 

It will use your openai token, and since it is a timed process it may ramp up usage costs (at your risk).

The grand plan is to eventually collect anonymized images and train a specialized model instead of using a foundational model. 
But for now, just a proof of concept for fun. 
