## Lab 3 - Quiz Configurator
I have developed an application for configuring quiz questions and running quiz rounds. The app is built in WPF and XAML, following the Model-View-ViewModel (MVVM) architecture. The application primarily consists of two parts: a "configurator" for creating question packs and a "player" for running quiz rounds.

## MongoDb
When you run the application the app will create a codefirst-database which you can see if you have MongoDbCompass on your desktop.

## Configuration Mode
You can create "Question Packs," i.e., packages of questions. You can add, remove, and edit existing questions. All questions have four options, with one being correct. You can also adjust settings for the packs themselves under "Pack Options": choose a name, tag the pack with a difficulty level, and set a time limit for the questions. Additionally, you can create new packs and delete existing ones.

There are multiple ways to add/remove questions: via the menu, through keyboard shortcuts, and using buttons in the app. The "Pack Options" menu can also be opened using all three methods.

## Play Mode
When starting Play Mode, the app displays how many questions are in the active "Question Pack" and which question is currently being answered in sequence. You can answer all the questions in the pack before receiving a result that shows how many were answered correctly. The order in which the questions are displayed is randomized each time, as is the order of the answer options.

A timer counts down (the time limit per question can be set during pack configuration). After selecting an answer (or if time runs out), the user receives feedback on whether their answer was correct/incorrect, along with the correct answer.

## Menu
The menu includes icons for the various options (e.g., using Font Awesome). Menu options can also be activated using keyboard shortcuts or the Alt key, such as Ctrl+O or Alt+E (O for "Pack Options").
