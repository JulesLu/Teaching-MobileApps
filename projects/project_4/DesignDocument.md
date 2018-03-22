# Image Guesser
In this app a user is able to take a live picture and then have the google api "guess" or tell you what your picture is. 
I expect it to be used to give people an idea of what they are looking at and then take user info to then see if it guessed a users
picture correctly. Then allows the user to start the app back over again. Maybe with more implementations it can become an I spy game
or could give a detailed description of what your image is. 

## System Design 
Android 5+
Camera
Google API
Bitmap

## Usage
A user will open the app and be prompted to open the camera. From here the user is able to take a live picture, they are able to 
choose if they want to keep this picture or take another one. The googe Api will then be used and will give a "guess" as to what
your image is a picture of. While doing so the app while say, "Please wait while we analyze your picture...". This takes anywhere from 10-15
seconds, the user is then allowed to say if the description does or doesn't match the users picture. These then go on to other layouts 
which say success or sorry lets try again. 
