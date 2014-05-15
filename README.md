FactorIt
========

### Dependency Injection (DI)
The act of providing dependencies to a piece of code so that they can be replaced easily.

### Factoring
The act of breaking apart a larger entity into a subset of smaller ones.

### Factory
A pattern used to create instances of an object.

Abstract
--------

FactorIt is cross-platform dependency injection made easy. This DI container is based on standard containers and exposes its features through a set of generic and lambda oriented methods that works together using a simple fluent interface. Its main goal is to stay as unobtrusive as possible. It needs very little configuration to get started and to inject dependencies. Even on a large scale or when using advanced features. No hacks. No magic. Just fast and portable.


Goodies
-------

- FactorIt is also available on [NuGet!](https://www.nuget.org/packages/FactorIt/ "Click here to see the latest NuGet package")
- Need some C++ love? Take a look at our C++ port: [FactorIt++](https://github.com/FurryBuilder/FactorItpp)

Basic Scenarios
-------------------

### Configuration
To create a root container, simply use the `CreateRoot()` static method. This ensures that root containers are clearly identified in your code.

    var container = Container.CreateRoot();

Then, you can simply bind any interface to a factory using the `Bind/To` fluent syntax provided by the container.

    container
        .Bind<IService>()
        .To(_ => new ConcreteService());

If you need to pass the container around, you can either use the `IBindingRoot` interface for a write only container or the `IServiceLocator` interface for a read only container.

The `IContainer` and `IContainerNode` interfaces are more of an internal thing but must be exposed publicly because of the way child containers are linked together.

### Usage

To extract a dependency from the container, you can simply use the `Resolve` method. On the first call, this will create an instance of the bound type using the provided factory. Subsequent calls will resolve to the same instance. Resolving is a thread safe operation.

    var aConcreteService = container.Resolve<IService>();

Advanced Scenarios
----------------------

### Keys
As the amount of services in the container grows, you might see the need to register the same interface multiple times but with different factories. For instance, when using Reactive Extensions, you might want to register the `IScheduler` interface for each type of schedulers your application requires. The `Bind` method enables you to provide an optional key parameter for those cases.

    container
        .Bind<IScheduler>("background")
        .To(_ => ThreadPoolScheduler.Instance);
    container
        .Bind<IScheduler>("immediate")
        .To(_ => ImmediateScheduler.Instance);

You can also provide a default instance, when no key is provided, or an alias by using the service locator provided as the first parameter of the factory.

    container
        .Bind<IScheduler>()
        .To(l => l.Resolve<IScheduler>("background"));

### Decorators
If decorators are your thing, you can also specify a decorator to wrap around your service.

    container
        .Bind<IService>("aDecoratedService")
        .To(l => ConcreteService(l.Resolve<IScheduler>()))
        .Decorate(iservice => new Decorator(iservice));

For cases when the decorator type might not be known at configuration time, you can also inject it directly from the container.

    container
        .Bind<IService>("aDecoratedService")
        .To(l => ConcreteService(l.Resolve<IScheduler>()))
        .Decorate((l, iservice) => l.Resolve<IDecorator>(iservice));

### Notifications
Sometimes, you will also need to schedule operations to be executed when a specific contract is registered. This could be useful if you insert plugins dynamically into the container or if you simply need to notify your log manager that a new logging destination has been registered. This can be done using the `Postpone` method.

    container.Postpone<IService>(iservice => Manager.ScanContainer());

### Child Containers
You can also create child containers by using the `CreateWithParent()` static method and passing in the parent container as a parameter. Since this method only requires an instance of the service locator to work, you can either create dangling child containers...

    var child = Container.CreateWithParent(container);

... or even sub container!

    container
        .Bind<IServiceLocator>("child")
        .To(l =>
        {
            var c = Container.CreateWithParent(l);

            c.Bind<IService>.To(_ => new ConcreteService());

            return c;
        });

### Scope
When using child containers, resolving services becomes a lot more powerful. The concept of scope built into the FactorIt container enables tons of different injection scenarios. For instance, you might be in a situation where you know where a contract might be registered. This is why the `Resolve` and `Postpone` methods provides overrides which includes a scope parameter. The scope is an abstract way of limiting which containers in the tree will participate in the resolving operation.

    container.Resolve<IService>(Scope.Downward);        // Parent + Local
    container.Postpone<IService>(Scope.Upward, ...);    // Local + Children

These are just two examples of the possible policies that can be provided when dealing with child containers. You can also limit the accessibility of services at configuration time by using multiple container branches.

    var r = Container.CreateRoot();
    var sensitive = Container.CreateWithParent(r);
    var freeforall = Container.CreateWithParent(r);

Using this technique, even if child containers are created under the freeforall container, and no matter how wide the scope resolving rules are, no one will ever be able to access the sensitive services. This is perfect when exposing a plugin structure where only the API facing services needs to be accessible.

### Cleanup
Finally, probably the coolest feature of this container is its ability to cleanup after itself. If a registered service implements the `IDisposable` interface, it will call the `Dispose` method on this service. This will only happen if the service has been resolved at least once. It will also dispose all child containers automatically so you only need to call `Dispose` on the root container. Currently, services are disposed in no particular order within containers and containers are disposed one child branch at a time. Take a look at the pipeline section to see what's comming up!


Pipeline
--------

In the pipeline, we plan on greatly improving the cleanup procedure by adding a system to detect service dependencies. This will enable automatic disposal of dependent services and will correctly call the `Dispose` method in their dependent order across containers.
