# Nostale Gfless

- More information about how custom launchers for nostale work can be found here: [in morsisko's readme](https://github.com/morsisko/NosTale-Gfless/blob/master/README.md)

## How to use this library

- You can see real CLI example in NostaleGfless.Example

```
var authenticator = new GameforgeAuthenticator();
var launcher = await authenticator.Authenticate(email, password);

launcher.Launch(launcher.Accounts.First()).ContinueWith(data => {

  // This will be called either after NosTale main window was opened or the process was closed

  if (data.Initialized)
  {
    Console.WriteLine("Yay! NosTale started!");
  }
  else
  {
    Console.WriteLine("Oops! There was an error :(");
  }
});
```

## NostaleGfless.Example

- The example is a simple CLI which can be used to log in to the game

```
NostaleGfless.Example 1.0.0.0
Copyright c  2020

  -i, --installation    Installation guid. Can be obtained from regedit.

  -n, --nostale         Required. Path to nostale folder or NostaleClientX.exe

  -a, --account         Name of the account to connect to. Otherwise the first
                        one will be used. For more accounts split them using
                        coma

  --help                Display this help screen.

  --version             Display version information.

  Email (pos. 0)        Required. Gameforge account email

  Password (pos. 1)     Required. Gameforge account password

```

> Example usage: `NostaleGfless.Example.exe -n "C:\Program Files\NosTale\de-DE" -a myAcc1,myAcc2 myemail@asdf.com myPassword`

## Thanks to

- **morsisko** and his [Gfless](https://github.com/morsisko/Nostale-Gfless) library