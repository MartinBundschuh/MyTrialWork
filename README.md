# lync-caller

In this sample, it is shown how to youse LyncClient to start calls. 
Therefore the MVVM Patteren is used aswell as Entity Framework

*Projekt .<b>Data</b>

The Class LyncCall:
- Start the call itself. It is possible to call people within ones contactlist or any other number.
- Group calls are possible.
- All calls are logged in the database (Entity Framework)

The Class PhoneNumber:
- Is used to pass LyncCall a defined phonenumber syntax.
- Returns a formated phonenumber which can be used in GUIs.

ExceptionClasses:
- Custom ExceptionClasses for more detailed exception handling.
- Different Exceptions for false e-mail addresses, phonenumbers or lync-call processes.

Call Logging:
- Outgoing calls will be logged in the database.
- Callinformations: From, To, Date/Time, call to user in contactlist (true/false).
- Etity Framework implemented with a simple datatable and localhost.


*Projekt .<b>Tests</b>

TestClasses:
- Just a few standard UnitTests.
- Only basic tests to show how to possibly use them.


*Projekt .<b>UI</b>

LyncCallWindow
- A simple WPF Window. Using Databindings and a simple grid.
- Phonenumber can be entered and a call started.

LyncCallViewModel
- Starts the acutal Lync-call.
- Holds the data of the LyncCallWindow.
