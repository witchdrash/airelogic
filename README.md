# Running
Easiest way to get this running is to compile and run in Visual Studio, then use the Swagger UI that should open automatically, if it doesn't you can get there by accessing `/swagger`, normally since the site loads on port 5000 this should be `http://localhost:5000/swagger`.

This can also be run from the command line by navigating to the `LyricInfoApi` folder and typing `dotnet run`, this will cause the application to run in the commandline on port 5000.

The tests can be executed from the command line as well by navigating to the `LyricInfoApi.Test` folder and typing `dotnet test`

There is a single newman test, this was to validate I was getting the artists out correctly, this can be run by using `npm run newman` you will need to run an `npm install` if you don't have newman installed globally.

I would have added one for the other endpoint, since there was no direct acceptance criteria and I couldn't find an artists with few enough songs that manually calculating what the average should be wouldn't take an incredibly long time and be highly error prone, I was concerned adding one would simply retain a potentially buggy piece of code, which means the test produces negative value by giving a false green, and given that this was an excercise to demonstrate how I worked with APIs rather than being production code I decided against writing it.

# Use
First step is to use the artist search api to find the artist id
Second step is to use that id in the second call to do the average calculation, depending on your artist this will take between a long time and a very long time.
