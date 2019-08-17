# Universal Resolver

Programmer &amp; document writer:  Vinh Vu
Contact email:     [Mrthanhvinh168@gmail.com](mailto:Mrthanhvinh168@gmail.com)
My public repositories:    [https://github.com/game-libgdx-unity](https://github.com/game-libgdx-unity)
Version:     1.0.6

Thanks for trying Universal Resolver!

Unity3d provides you a few ways to resolve your dependencies, however Universal Resolver even make it more easy and reduce your repetitive code. Inspired from Reactive Programming &amp; Redux State Management. Universal Resolver is an advanced &amp; automatic IoC Framework built on Generic &amp; Reflection API, which is a combination of binding settings as scriptable objects, convenient static class-level API,  along with C# Attributes which allow you to resolve any fields, constructors, methods or properties as [Component], [Singleton], [Prefab] or [Transient]. In this docs, we will refer &quot;Dependency Resolver&quot; as &quot;Resolver/IoC&quot; For short.

In the folder &quot;Assets/DependencyResolver/Samples&quot; There are a lot of test scenes. There is a minesweeper game developed with this framework, which looks like below:

You can read this documentation online with latest updates at:

[https://docs.google.com/document/d/e/2PACX-1vRICY-Xj7f-Tlg0TLgALpe24w6IjPIrpFTPIwKZfxUtrVD8IV74Gb9qVOqm12\_ORu02is9b7cf6u1YY/pub](https://docs.google.com/document/d/e/2PACX-1vRICY-Xj7f-Tlg0TLgALpe24w6IjPIrpFTPIwKZfxUtrVD8IV74Gb9qVOqm12_ORu02is9b7cf6u1YY/pub)

This asset supports

- --Resolve both C# &amp; Unity Component objects
- --Build games on Editor, Standalone, iOS, Android and WebGL
- --Write loose-couple and testable C# code.
- --Use binding setting files as ScriptableObject files to setup binding.

You just need to change your binding setting files, then reload your scene to have changes applied. No recompiling needed.

Here are tutorials about using this asset. You can email us for any questions.

# Getting Started

1. Download from github for latest features and bug fixes: [https://github.com/game-libgdx-unity/Universal-Resolver](https://github.com/game-libgdx-unity/Universal-Resolver) or Get this asset for free at unity3d asset store: [http://u3d.as/1rQJ](http://u3d.as/1rQJ)
2. Download &amp; import the folder &quot;Assets/Dependency Resolver&quot;  to your projects.
3. Locate the Sample folder inside your project panel.
4. Open each of scene file in this folder to see how it works, and also to follow this document.
Don&#39;t worry, the sample code is extremely simple and well-documented with a lot of comments.
5. This project is based on Test Driven Development (TDD) whose tests can be run in Unity Test Runner.

# Static class-level API

This asset provides you many useful methods to bind or resolve objects.

- Context.GetDefaultInstance: Get or Initialize a shared static instance of Context
- Context.Bind\&lt;Abstract, Concrete\&gt;(): binding a concrete class for an abstract type
- Context.Resolve\&lt;ClassName\&gt;(): Resolve any class to give you the instance of the type \&lt;ClassName\&gt;, C# object or Unity components are both supported.
- Context.Dispose(obj): Dispose an obj, remove it from internal resolved object caches then trigger the subject OnDisposed
- Context.OnResolved: This is a reactive property that will push notifications when you use the static api from Context.Resolve\&lt;ClassName\&gt;() to resolve any object.
- Context.OnDisposed: This is a reactive property that will push notifications when you use the static Dispose() method to dispose an object.
- Context.OnInitialized: This is an event which will be called right after context is initialized. You can use it for calling Resolve\&lt;T\&gt; methods or setup your pools of objects.

# Validate Data

You can add constraints to data changes, that work like middleware inside your application

var maxMsg = &quot;Maximun count of IAstract reached&quot;;

Context.AddConstraint(typeof(IAbstract), (ref object o) =\&gt;

            {

                var countAbstract = Pool\&lt;IAbstract\&gt;.List.Count;

                if (countAbstract \&gt;= 2)

                {

                    return false;

                }

                return true;

            }

            , maxMsg, When.BeforeResolve);

Or

 var minMsg = &quot;Minimum count of IAstract reached&quot;;

        Context.AddConstraint(typeof(IAbstract), (ref object o) =\&gt;

            {

                var countAbstract = Pool\&lt;IAbstract\&gt;.List.Count;

                if (countAbstract \&lt;= 2)

                {

                    return false;

                }

                return true;

            }

            , minMsg, When.BeforeDelete);

Supported operations are BeforeResolve | AfterResolve| BeforeUpdate | AfterUpdate | BeforeDelete | AfterDelete.

If the new updated data is failed to validate against data constraints, then it will be reverted back and Context will raise an event with type of &quot;InvalidDataException&quot; as an observable

Sample usage:

        Context.OnEventRaised\&lt;InvalidDataException\&gt;().SubscribeToConsole(&quot;Exception&quot;);

# Update/Delete Data with Id

If your data implements  theIBindID interface, then you can update/delete model with its respective id.

# Important C#  [Inject] Attributes

Instead of calling &quot;Context.Resolve\&lt;ClassName\&gt;():&quot;, you can use C# inject attributes to resolve objects at the runtime when [Context] is initializing.

There is a general way to resolve any fields in your class is using [Inject] Attribute, think of it like this:

Unity code: Monster data; //have to construct the data by code, since it&#39;s null right now.
Universal Resolver: [Inject]Monster data;//the data now is constructed at the runtime, no more code needed

There are several attributes you can use:

- Inject: The General way to inject most of the types, try to get objects from context dictionary caches. If nothing found in the cache, then It&#39;ll try to resolve objects as [Component],  [Transient] or [Prefab].
- BaseInject: Base class for all inject attributes, it can be used if you specify the LifeCycle and Path.
- Component: Add or Get component or array of components from current gameObject or from a gameObject that is given by a path on hierarchy.
- Descendants:  Same as [component], however It&#39;ll process all descendants of targeted gameObject
- Ancestors: Same as [component], however It&#39;ll process all ancestors of targeted gameObject
- Children: Same as [Component], however [Children] works on the children of targeted gameObject.
- Singleton: Resolve as singleton, it means all references by [Singleton] will refer to the same object
- Transient: Resolve as a new brand C# object , or a new component on a new GameObject on Scene.
- Cache: Resolve by searching from context caches, if nothing found, then resolve objects as [Default].

Also there are additional attributes that don&#39;t resolve objects but  help [Context] work more precisely

- ProcessingOrder: Customize order of processing objects, the low values will be processed first.
- IgnoreProcess : Context will never process a class which marked by [IgnoreProcess].
- Override  : which force [Context] to resolve a field even if the value of the field isn&#39;t default value.
- Binding: Bind this class for a custom abstract class, then register it inside the Context.

# Important C# Interfaces for [Inject] attributes

To extend the functions of object resolving, we provide you several useful interfaces that you can implement in your class, in order to get some of supported features from the [Context]. Those interfaces are using for  [Inject] attributes

- IComponentResolvable: Try to get a component out of a MonoBehaviour, return null if not found
- IComponentArrayResolvable: Try to get the needed components out of a MonoBehaviour as array.
- IObjectResolvable: This should be implemented in mono behaviour to get a non-component when processing a mono-behaviour. Also this can be implemented in inject attributes to get a ScriptableObject which is loaded from Resources folder by a given Path.

# Important C# Interfaces for C# class / monobehaviours

There are some concept about data &amp; view in Universal Resolve:
A Data layer should only contain layer, a raw C# class with only variables, no logic should be included.
A View layer should be mono behaviours, which represent the data and send commands from users in order to modify the data.

- IViewBinding\&lt;T\&gt;:  Bind a Data layer with a View Layer, View will be created if the linked data object is resolved by [Context].
- IDataBinding\&lt;T\&gt;: Bind a View layer with a Data Layer,  View will have the data object \&lt;T\&gt; after its creation, passed on OnNext(T data) method of this interface. You can use the method to initialize View.

IViewBinding and IDataBinding support multiple argument generic types, e.g: you can write it like this to link 2 views for 1 data:

class Data : IViewBinding\&lt;V1, V2\&gt; {}

or to link 2 data objects for 1 view:

class View : Monobehaviour, IDataBinding\&lt;D1, D2\&gt; {
 void OnNext(D1 data){}
 void OnNext(D2 data){}
}

# Understand how Data-View-Business Layers

Universal Resolver encourages you to apply the mvc architecture to your unity game. They are: Models which contain data. Controllers which modify the data and Views, which render data visually to app users.

Inside the project, the is a sample game which is an automatically self-resolvable minesweeper. The game is initialized by a field of cells that may contain mines, the mission is exposing all the mines hidden inside the cells. It is separated by models/data layer, Controllers/businesses and the View/UI layer,

[data]
[https://github.com/game-libgdx-unity/Universal-Resolver/blob/source-only/Assets/DependencyResolver/SampleGame/Scripts/Cells/CellData.cs](https://github.com/game-libgdx-unity/Universal-Resolver/blob/source-only/Assets/DependencyResolver/SampleGame/Scripts/Cells/CellData.cs)

the [Data] only contains data, no business included, also I use reactive property from UniRx then linked them to UI layer, so when the business level doing its job, the data will be modified, then the UI should be auto-updated by the data changes.

[UI / View]
[https://github.com/game-libgdx-unity/Universal-Resolver/blob/source-only/Assets/DependencyResolver/SampleGame/Scripts/Controls/Cell.cs](https://github.com/game-libgdx-unity/Universal-Resolver/blob/source-only/Assets/DependencyResolver/SampleGame/Scripts/Controls/Cell.cs)

This is the view of the CellData, View should be a mono-behaviour to represent the cell status, then It updates the UI visually (Text, color, background, etc) to the Users. Also no business included in UI layer.

UI layer can handle events which will make business objects react to.

[Business]
[https://github.com/game-libgdx-unity/Universal-Resolver/blob/source-only/Assets/DependencyResolver/SampleGame/Scripts/Boards/GameBoard.cs](https://github.com/game-libgdx-unity/Universal-Resolver/blob/source-only/Assets/DependencyResolver/SampleGame/Scripts/Boards/GameBoard.cs)

[GameBoard] is used to create a field of cells with method Build(width, height, minesNum), modify the cell state with OpenCell(x, y) and then to avoid hitting bombs at the first move, the FirstMove(x, y) is used to open any first cell with no bombs.

[https://github.com/game-libgdx-unity/Universal-Resolver/blob/source-only/Assets/DependencyResolver/SampleGame/Scripts/Boards/GameSolver.cs](https://github.com/game-libgdx-unity/Universal-Resolver/blob/source-only/Assets/DependencyResolver/SampleGame/Scripts/Boards/GameSolver.cs)

Also we had [GameSolver] which will run algorithms to mark cells have bombs base all the current status of opened cells. If it can&#39;t give a decision then a random cell will be opened and the gameSolver may fail to resolve the game if that cell has bombs.

[Universal resolver] encourages you to apply this Data/View architectures to your projects. Instead of writing a long class or method, should put the code into many methods/classes, follow by the architecture &amp; patterns, then reduce number of methods/classes need to be modified to implements your new features.

In your team, before starting a project, the software architecture should be discussed to make sure everyone can understand. Define general rules, coding conventions, models, businesses, screens, etc.

# How Dependency Resolver works

There are two approaches to resolve objects in Dependency Resolver

1. Get existing objects on Scene / resources folder / assetbundles by a given path (System.String).

In the constructors of Inject Attributes, you can set a string for Path parameter, the [Context] will be looking for a gameObject by the given path on current scene. If the object not found in scenes, IoC will search in loaded asset bundles, and finally searching in Resources folders.

[Component(&quot;child/child&quot;)] private TestComponent[] testComponents;

[Component(&quot;child/child&quot;)] TestComponent testComponent;

[Transient(&quot;child/child&quot;)] private TestComponent aTestComponent;

[Singleton(&quot;child/child&quot;)] private TestComponent sTestComponent;

In above code, needed components will be searched from a gameObject from current transform/child/child

1. Create new object by Activator, or clone a component from a gameObject in Resources folder or
downloaded/decompressed asset bundles by a given C# class (System.Type).

//create context which will automatically load binding setting by the assembly name

//in this case, please refer to SceneTest setting from the resources folder.

var assemblyContext = new AssemblyContext(GetType());

//try to resolve the object by the default settings of this SceneTest assembly

var obj = assemblyContext.Resolve\&lt;AbstractClass\&gt;(LifeCycle.Singleton);

var obj2 = assemblyContext.Resolve\&lt;AbstractClass\&gt;(LifeCycle.Singleton);

var obj3 = assemblyContext.Resolve\&lt;AbstractClass\&gt;(LifeCycle.Transient);

The sample code will resolve the &quot;AbstractClass&quot; as respective Concrete Type which specified from binding setting files.

\* Combination of two approaches

If the 1. approach fails to resolve object, the 2. will take action. The combination of two approaches will likely be able to resolve any object at the runtime.

For better performance when you resolve a ton of objects, Dependency Resolver uses a built-in cache system in searching of Unity Objects in Scenes / Resources / Bundles / Registered Objects.

# Create your unity assembly definition files

It&#39;s not mandatory to create C# namespace and unity assembly definition files to use this asset.

However we would suggest creating them, since they will help organize your source code effectively.

You can refer to this docs to see what is the unity assembly definition files

[https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html](https://docs.unity3d.com/Manual/ScriptCompilationAssemblyDefinitionFiles.html)

After you created these unity assembly definitions, now it&#39;s time to create binding setting files.

# Create binding settings.

You can create your own binding setting by go to menu

&quot;Assets/Create/IoC/Binding Setting&quot; to create a new binding setting file.

You can see there are some fields in a binding setting from inspector panel:



You can bind a concrete class to an abstract/interface as Transient/Singleton/component as you prefer.

Also you can tick on Inject to show &quot;Resolve from type&quot; context.

For instance, the above picture, It will resolve:

- --any Abstract object as ImplClass if you resolve it inside TestComponent class.
- --any Abstract object as ImplClass2 if you resolve it inside TestComponent2 class.
- --any Abstract object as ImplClass3 if you resolve it inside other classes.

 You also can bind a GameObject to an Monobehaviour C# class as Prefab as you prefer. When you try to resolve the C# class, the IoC will return the GameObject for you.

 For instance, in the below photo:

When you resolve the type &quot;Cell&quot; which is a monobehaviour, the IoC will return the &quot;Cell&quot; prefab as specified in binding setting files.

# Context

This asset uses [Context.cs] class to resolve things.

AssemblyContext objects attach to the assembly definition files you created. They are used to resolve objects when you no need to resolve objects in a particular context. (inside some class, etc )

AssemblyContext can automatically find a binding setting files with the same name as the assembly.

In the sample code, you can see I created assembly named &quot;SceneTest&quot; and I have the setting for it in resources folder. They have the same name.

How to deal with default setting files (Which has the same name with assembly name) ?

Have a look at sample folder, open scene TestBindingSetting

They are running on an assembly name &quot;SceneTest&quot; with a monobehaviour below:

void Start () {

  //create context with automatic load binding setting from assembly name

  //in this case, please refer to SceneTest setting from the resources folder.

  var assemblyContext = new AssemblyContext(GetType());

  //try to resolve the object by the default settings of this SceneTest assembly

  var obj = assemblyContext.Resolve\&lt;AbstractClass\&gt;();

  //you should see a log of this action in unity console

  obj.DoSomething();

}

Run this code, you will resolve AbstractClass as ImplClass3 from SceneTest setting.

Also there are a lot of  sample to use the Dependency Resolver assets, please feel free to investigate them.

This is a sample of the code running in Unity:

# What is the IoC?

Inversion of Control (IoC) means to create instances of dependencies first and latter instance of a class (optionally injecting them through constructor), instead of creating an instance of the class first and then the class instance creating instances of dependencies. Thus, inversion of control **inverts** the **flow of control** of the program. **Instead of** the **callee controlling** the **flow of control** (while creating dependencies), the **caller controls the flow of control of the program**.

[https://stackoverflow.com/questions/3058/what-is-inversion-of-control](https://stackoverflow.com/questions/3058/what-is-inversion-of-control)

Don&#39;t worry if you still don&#39;t get it, I&#39;ll show you how it works by examples &amp; actions :)

# Why should I need an IoC?

Well if you have been using DI (Dependency Injection) for a while, maybe this is the first question you have. So why do you need an IoC container as opposed to straightforward DI code?

I can give you an example of DI:

In a good day, You just realize that you need an instance of ProductLocator for your ShippingService class.

Okay then create it by var pl = new ProductLocator();

Why you need to create it? Because you have another object is ShippingService and you need to pass a ProductLocator to the ShippingService&#39;s constructor!

So this is how you should create ShippingService: new ShippingService(new ProductLocator());

Wait, what if ShippingService&#39;s constructor have more than one parameter? E.g they have 5 or 10, even these parameters also have their own dependencies? And finally you may end up your code like this

var svc = new ShippingService(new ProductLocator(),

   new PricingService(), new InventoryService(),

   new TrackingRepository(new ConfigProvider()),

   new Logger(new EmailLogger(new ConfigProvider())));

Ridiculously crazy, right?

I know this example maybe too exceeding, just to show you benefits of an IoC. However if you have used an  IoC, then you can let the IoC create the instance of ShippingService for you simply:

IoC context = new IoC();

var svc = context.Resolve\&lt;ShippingService\&gt;();

Ridiculously simple, right?

As you can see everything done with the help from a simple IoC container, it resolves every dependencies of your objects for you magically.

That&#39;s one of goals I want to achieve from developing this project.

Other benefits can be

1. You write Loose coupling classes, they are dependent from each other.
2. Your code is testable, using unity test runner or other automatic test tools.
3. Testable code is the good code. If you are not sure why, you should google it.
4. Reduce lines of code you have to write (or copy &amp; paste)
5. You don&#39;t waste your time to resolve the dependency for your class anymore, let IoC handle it!

# How to get the source code?

All of my source code are free to download at my github, this repository
[https://github.com/game-libgdx-unity/Unity-IoC-inverse-of-control-dependency-injection-](https://github.com/game-libgdx-unity/Unity-IoC-inverse-of-control-dependency-injection-)

# Can I see a game built by using Dependency Resolver framework?

In the repository, you will see a unity scene located at Assets/IoC/SampleGame/Scene/Game.unity

It&#39;s a game version of mine sweeper written in Dependency Resolver

Before that, I already developed a similar project using Zenject, then I realized zenject is a overkill for what I need, so I started developing a new one more fit to my needs. Here is a link to the old project

[https://github.com/game-libgdx-unity/minesweeper](https://github.com/game-libgdx-unity/minesweeper)

# Where can I found more tutorials about Dependency Resolver?

I also made some videos on my youtube channel, but my voice isn&#39;t clear, so sorry about that.

[https://www.youtube.com/playlist?list=PLrxnIke4BNsTyVk2piv7PclE5aDadmfIB](https://www.youtube.com/playlist?list=PLrxnIke4BNsTyVk2piv7PclE5aDadmfIB)

Not only Dependency Resolver but I also talked about Photon server, Firebase, UniRx and many interesting things.

If you want to use Dependency Resolver, then I think my videos will be helpful for you.

## Create bindings

[Binding] should be placed before class keyword

[Binding(typeof(AbstractClassOrInterface)] means you will bind the abstract code to this conrete class when you use the context to resolve the abstract type.

## Resolve objects

There are some attributes to resolve object using binding you defined, they are singleton, transient, component, etc. They should be placed before a C# class member (fields, properties, methods, etc)

[Singleton] :  If you want to context resolve an object as singleton.

[Transient] :  If you want to context resolve an object as transient.

[Component] : used only for unity components. these objects will be get from the gameObject or created by adding a new component to the gameObject.

[Prefab]: A special singleton type of Component object. You can call Object.Instantiate() to clone any object which is resolved as [Prefab].

And more attributes to come,...

Instead of using attributes, you can just write a few lines of code to resolve anything:

AssemblyContext context = new AssemblyContext();

Var instance = context.Resolve\&lt;TypeOfClassYouWant\&gt;();

Then you should get the instance :)

# What if the instance return from context is null ?

well , in that case, first look at console to see if there is any exception thrown, I throw exceptions when there are invalid operations happened in the context.

You can copy the console logs then email me if you cannot solve the issue. Maybe something went wrong.

# What have I done so far to resolve dependencies in Unity3d?

Unity3d provides a few of ways to resolve your dependencies. Let&#39;s think about it when your mono behaviour need an instance of rigidBody!

1. You can use the modifier public fields or using [SerializeField] to expose non-public fields.

E.g: public Rigidbody rigidBody; [SerializeField] Rigidbody rigidBody

Then you have to select objects then drag &amp; drop them from unity editor to resolve the rigidBody dependency which your behaviour needs.

2. You can use the GetComponent or Find... static methods from UnityEngine.Object

Well, by this way, you can add or get component from the gameObject or from the other gameObjects in the scene.

They are convenient methods that Unity already provided, but you also write that code time to time,  and there are issues, e.g:  Find... methods only work for non-disable GameObjects while GetComponent cannot get the references for some &quot;Manager objects&quot; that could be a singleton.

# What&#39;s wrong with the Singleton?

I saw Many people love singleton, they say it&#39;s so convenient, (I think so too)

Also many people hate singleton, they say it&#39;s an anti-design pattern. (Maybe they don&#39;t tell you why)

Personally I have been using singleton for quite a long time, I also wrote generic singleton classes for  unity, you can check it here

[https://github.com/game-libgdx-unity/TD/blob/master/Assets/Scripts/Core/SingletonBehaviour.cs](https://github.com/game-libgdx-unity/TD/blob/master/Assets/Scripts/Core/SingletonBehaviour.cs)

However, using singleton is not best practice in programming. I can tell you I saw so many teams who have to fix their bugs hopelessly just because they all using SINGLETON which are public static accessible from every where in their code.

Without using it properly, Singleton will be the root of the code smell.

You also are probably writing the same &quot;manager objects&quot; using singleton. &quot;Game manager, sound manager, Level manager, etc&quot;.

Singleton is only one instance in the program, and other objects can access the singleton by a shared static instance.

It means you cannot create a second one. E.g, I want to test GameClient class by creating 2 clients and let them communicate each other. If you write the GameClient as singleton, it&#39;s impossible to even write a unit test for the that functionality.

You also will not have abstraction, polymorphism and inheritance for your OO classes. Therefore your code is not a true OOP!

By modifying the singleton from everywhere, you are making your code vulnerable for side effects which can cause the serious bugs in common cases.

You can not change the behaviour of your code at the runtime, you have to edit the source and recompile, then run again the application to see your changes.

If you still want to use singleton, it&#39;s your decisions, I won&#39;t make you stop using it.

# Licence and contribute to this project?

It&#39;s free to use in your projects, however I do not allow to redistribute the source code in any case, and  I want to know if this IoC is helpful for your projects, please send me an email about your development when you use my source code. The same way if you want to contribute for Dependency Resolver.

Thanks for reading!
