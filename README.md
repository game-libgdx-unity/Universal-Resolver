# Unity-IoC
Implements for Unity in fields of IoC, Reactive programming, Firebase, Photon Server, etc.

This project is under an active development and subjected to be changed, got bugs fixing and maybe many unknown threats. Using source code from this project is at your own risk!

You can read a full version with nice formated at https://docs.google.com/document/d/1xVOoFa0KxrTnqfogQ_s9qoCgWL5meJnlkENV4_WaylE/edit?usp=sharing

I also made some videos for this on my youtube channel, 
https://www.youtube.com/playlist?list=PLrxnIke4BNsTyVk2piv7PclE5aDadmfIB 
(my voice isn’t clear, so sorry about that)

Unity IoC User's Guide
Programmer & Document writer: Vinh Vu Thanh
Contact email: Mrthanhvinh168@gmail.com
My public repositories: https://github.com/game-libgdx-unity 
What is IoC?

I found an answer on stackoverflow
Inversion of Control (IoC) means to create instances of dependencies first and latter instance of a class (optionally injecting them through constructor), instead of creating an instance of the class first and then the class instance creating instances of dependencies. Thus, inversion of control inverts the flow of control of the program. Instead of the callee controlling the flow of control (while creating dependencies), the caller controls the flow of control of the program.
https://stackoverflow.com/questions/3058/what-is-inversion-of-control 
Don’t worry if you still don’t get it, I will explain it by examples & actions :)
Then why should I need an IoC?

Well if you have been using DI (Dependency Injection) for a while, maybe this is the first question you have. So why do you need an IoC container as opposed to straightforward DI code?
I can give you an example of DI: 
In a good day, You just realize that you need an instance of ProductLocator for your ShippingService class.
Okay then create it by var pl = new ProductLocator();
Why you need to create it? Because you have another object is ShippingService and you need to pass a ProductLocator to the ShippingService’s constructor!
So this is how you should create ShippingService: new ShippingService(new ProductLocator());
Wait, what if ShippingService’s constructor have more than one parameter? E.g they have 5 or 10, even these parameters also have their own dependencies? And finally you may end up your code like
var svc = new ShippingService(new ProductLocator(), 
   new PricingService(), new InventoryService(), 
   new TrackingRepository(new ConfigProvider()), 
   new Logger(new EmailLogger(new ConfigProvider())));
I know this example maybe too too exceeding, just to show you benefits of an IoC. I mean if you have an IoC, then you let the IoC create the instance of ShippingService for you:
IoC context = new IoC();
var svc = context.Resolve<ShippingService>();
As you can see everything done with the help from a simple IoC.
That’s one of things I want to archive from developing this project!
Other benefits can be
You write Loose coupling classes, much easier to write unit tests.
Your code is testable, using unity test runner or other automatic test tools.
Testable code is the good code. If you are not sure why, you should google it :)
Reduce number of lines of code you have to write (or copy & paste)
You don’t waste your time to resolve the dependency for your class anymore, let IoC handle it!
How to get the source code?

All of my source code are free to download at my github, this repository
https://github.com/game-libgdx-unity/Unity-IoC-inverse-of-control-dependency-injection- 
Can I see a demo for this project?

In my repository, you will see a unity scene in Assets/App/Scene/Game.unity
It’s a mine sweeper version written in unity IoC 
Before that, I already developed that game using Zenject, then I realize zenject is a overkill for my projects, so I’m developing a new one more fit to unity. Here is the old project
https://github.com/game-libgdx-unity/minesweeper 
Where can I found more tutorials?

I also made some videos on my youtube channel, but my voice isn’t clear, so sorry about that!
https://www.youtube.com/playlist?list=PLrxnIke4BNsTyVk2piv7PclE5aDadmfIB 
How to get started?

Get the project, then using UnityIoC; namespace in the top of your C# files.
You might want to create a context
Context context = new Context();
Now we have the context, that will resolve most of objects for your classes. 
I intend to make C# attributes to decorate your code! Let me show you some of them!
Create bindings
[Binding] should be placed before class keyword
[Binding(typeof(AbstractClassOrInterface)] means you will bind the abstract code to this conrete class when you use the context to resolve the abstract type.
Resolve objects
There are some attributes to resolve object using binding you defined, they are singleton, transient, component, etc. They should be placed before a class member (fields, properties, methods, etc)
[Singleton] :  If you want to context resolve an object as a singleton. 
[Transient] :  If you want to context resolve an object as a singleton.
[Component] : used only for unity objects. these objects will be get from the gameObject or created by adding new component to the gameObject
And more to come,...
Instead of using attributes, you can just write a line of code to resolve
Context context = new Context();
Var instance = context.Resolve<TypeOfClassYouWant>();
Then you got the instance :)
What if the instance return from context is null ???

well , in that case, first look at console to see if there is any exception thrown, I throw exceptions when there are invalid operations happen in the context. 
You can copy the unity logs then email me if you cannot solve the issue.  Maybe something went wrong.
What have you done so far to resolve dependencies in Unity3d?

Unity3d provides a few of ways to resolve your objects/fields/dependencies. Let’s think about it when your mono behaviour need an instance of rigidBody!
1. You use the modifier public or using [SerializeField] for your non-public members.
E.g: public Rigidbody rigidBody; [SerializeField] Rigidbody rigidBody
Then you have to select/Drag&drop the object from unity editor to resolve the rigidBody dependency you need.
2. You use the GetComponent or Find... static methods from UnityEngine.Object
Well, by this way, you can add or get component inside the gameObject or from the others in the scene.
Yes, You got convenient methods that Unity already provided, but you also write that code time to time,  and there are issues, e.g:  Find... methods only work for non-disable GameObjects while GetComponent cannot get the references for some “Manager object” that could be a singleton.
What wrong with the Singleton ???

Nothing wrong with it, in fact I have been using singleton for quite a long time, I also wrote a generic singleton classes for  unity, you can check it here
https://github.com/game-libgdx-unity/TD/blob/master/Assets/Scripts/Core/SingletonBehaviour.cs 
However, using singleton is not a best practice in programming. I can tell you I have shown so many teams have to fix their bugs hopelessly just because they all using SINGLETON which are public static accessible from every where in their code. You also are probably writing the same “manager objects” using singleton. “Game manager, sound manager, Level manager, etc”.
Singleton is only one instance in the program, and other objects access the singleton by a shared static instance. 
It means you cannot create a second one! E.g, I want to test GameClient by creating 2 clients and let them communicate each other. If you write GameClient as singleton, it’s impossible to even write a unit test for the that functionality.
You also will not have polymorphism and inheritance for your OOP.
You expose your code vulnerability for side effects which cause the serious bugs in many cases.
You can not change the behaviour of your code at the runtime, you have to edit the source and recompile, then run again that static instance.
If you still want to use singleton, it’s your decisions, I won’t force you to stop using it.
Licence and contribute to this project?

It’s free to use, however I do not allow to redistribute the source code in any case, and  I want to know if this repository helpful for your projects, please send me an email about your development when you use my source code. The same way if you want to contribute for unity IoC.

Thanks for reading!
