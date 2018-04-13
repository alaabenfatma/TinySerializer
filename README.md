# TinySerializer
TinySerializer is a .NET objects serializer. It is written in approx. 80 lines of code only. 
The main goal of this basic serializer is to demonstrate the basic functionalities of a de/serializer.
## Implementation
code:

```csharp
        private class Pet
        {
            public string Type { get; set; }
            public string Name { get; set; }
            public string Age { get; set; }
            public double Weight { get; set; }
        }
        /*
        Input :
        Type = Dragon
        Name = SkyCloud
        Age = 9200 years
        Weight = 9562.6500
        */
```
      
Generated code:
```php
<?
[Type=Dragon]
[Name=SkyCloud]
[Age=9200 year]
[Weight=9562.6500]
?>
```

## Notes
* This serializer may possibly be the tiniest serializer around.
* It is basic; basic as heck.
* It does not support arrays/collections of objects. However, workarounds can be implemented to perform such a feat manually.
