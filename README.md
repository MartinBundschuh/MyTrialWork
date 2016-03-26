# MyTrialWork
Some few code stuff to show a little of my programming skills

In this sample, it is shown how to youse LyncClient to start calls.

The Class LyncCall:
- Start the call itself. It is possible to call people within ones contactlist or any other number.
- Group calls are possible.
- All calls are logged in the database (Entity Framework)

The Class PhoneNumber:
- Is used to pass LyncCall a defined phonenumber syntax.
- Returns a formated phonenumber which can be used in GUIs.

UnitTests:
- Just a few standard UnitTests.
- Only basic tests to show how to possibly use them.

ExceptionClasses:
- Custom ExceptionClasses for more detailed exception handling.
- Different Exceptions for false e-mail addresses, phonenumbers or lync-call processes.

Call Logging:
- Outgoing calls will be logged in the database.
- Callinformations: From, To, Date/Time, call to user in contactlist (true/false).

LyncSampleUser:
- Contains a simple WPF Window.
- Phonenumber can be added and a call started.
- Simple exceptionhandling.
