# ObjectChanged
Compare two objects and detect if they are different. Uses attributes and reflection. Compatible with Entity Framework.

## Motivation
I need an easy way to compare two objects for changes. My use case is mostly for MVC web apps where you have the posted object from the user
and the oject as is from the database during a post.

In my scenerio, if certain fields changed, I needed to create a new "version" of the object in the database. 
This is for change tracking at the user/business level.

## Design
I designed this library for maximum flexibility. There are two nice features: attribute usage and comparing by name and ignoring 
the parent type. 

The ChangesAttribute is used to mark the field or property that you want to track. This makes it easy to only compare a subset of fields.

I compare the fields/properties by name. So you can actually compare two objectrs with different classes as long as they have one 
or more matching properties/fields. See examples below for more details.

The third nice feature is comparing IEnumerable and IEnumerable<T> types. If you have two lists of elements,  you can compare them.
If they differ in any way, either by number or by comparing elements, then HasChanged is true. For this to work, you need to use the
ChangesId attribute and choose an identity field that uniquely identifies the object so that we can compare the correct objects in the list.

A nice addition would be to expand the identity check to more than one field. This would be nice for compound database keys. However
since most ORMs don't handle compound keys well, I've left that out of the first version. Also I don't need it currently. :-)

## Usage
Use the [Changes] Attribute to maker attributes that should be tracked for changing. Using an attribute turns out
to be the most flexible methood for specifiying which properties/fields to track.

### Basic Usage
```c#
public class ChangeTest
{
    [Changes]
    public int HasChangesAttribute { get; set; }

    public bool NoChangesAttribute { get; set; }
}

var orig = new ChangeTest { HasChangesAttribute = 1 };
var curr = new ChangeTest { HasChangesAttribute = 2 };

var changed = ObjectComparer.HasChanged(orig, curr);

/// changed = true
```

### Advanced - Compare different types
Because we use attribute for comparision and compare properties/fields by name, we can compare objects even if they have different types.

```c#
 public class ChangeTest
{
    [Changes]
    public int HasChangesAttribute { get; set; }

    public bool NoChangesAttribute { get; set; }
}

public class ChangeTest2
{
    [Changes]
    public int HasChangesAttribute { get; set; }

}

var orig = new ChangeTest { HasChangesAttribute = 1 };
var curr = new ChangeTest2 { HasChangesAttribute = 2 };

var changed = ObjectComparer.HasChanged(orig, curr);

/// changed = true
```

### IEnumerable Usage
This works for IEnumerable and the generic IEnumerable<T>
```c#
 public class EnumTest 
{
    [ChangesId]
    public int Id { get; set; }
    [Changes]
    public bool Changed { get; set; }
}

var newList = new List<EnumTest> { new EnumTest { Id = 1, Changed = true } };
var oldList = new List<EnumTest> { new EnumTest { Id = 1, Changed = false } };
var changed = ObjectComparer.HasChanged(newList, oldList);

/// changed = true
```

### IEnumerable Usage - Different types.
This works for IEnumerable and the generic IEnumerable<T>. Again, because we use attribute for comparision and compare properties/fields by name, 
we can compare objects even if they have different types.
```c#
public class EnumTest 
{
    [ChangesId]
    public int Id { get; set; }
    [Changes]
    public bool Changed { get; set; }
}

public class EnumTest2
{
    [ChangesId]
    public int Id;
    [Changes]
    public bool Changed { get; set; }
}

var newList = new List<EnumTest> { new EnumTest { Id = 1 } };
var oldList = new List<EnumTest2> { new EnumTest2 { Id = 2 }, };

var changed = ObjectComparer.HasChanged(newList, oldList);

/// changed = true
```


