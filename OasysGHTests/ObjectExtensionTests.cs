using System.Collections.Generic;
using OasysGH;
using OasysUnits;
using OasysUnits.Units;
using OasysGHTests.Helpers;
using Xunit;

namespace OasysGHTests
{
    [Collection("ComposAPI Fixture collection")]
  public class ObjectExtensionTests
  {
    [Fact]
    public void DuplicateTest1()
    {
      Force quantity = new Force(1, ForceUnit.Kilonewton);
      Force force = new Force(2, ForceUnit.Decanewton);
      IList<IQuantity> iQuantities = new List<IQuantity>() { Force.Zero, new Length(100, LengthUnit.Millimeter) };
      IList<Length> structs = new List<Length>() { Length.Zero, new Length(100, LengthUnit.Millimeter) };

      TestObject grandChild = new TestObject(true, 1.0, 1, "a", TestEnum.Value1, quantity, force, new List<TestObject>(), iQuantities, structs);
      TestObject original = new TestObject(new TestObject(grandChild));

      TestObject duplicate = (TestObject)original.Duplicate();

      duplicate.Children[0].Children[0].B = false;
      duplicate.Children[0].Children[0].D = -1.0;
      duplicate.Children[0].Children[0].I = -1;
      duplicate.Children[0].Children[0].S = "z";
      duplicate.Children[0].Children[0].TestEnum = TestEnum.None;
      duplicate.Children[0].Children[0].IQuantity = new Pressure(-1.0, PressureUnit.KilonewtonPerSquareMeter);
      duplicate.Children[0].Children[0].Force = new Force(-1.0, ForceUnit.Dyn);
      duplicate.Children[0].Children[0].IQuantities = new List<IQuantity>() { Force.Zero, new Length(100, LengthUnit.Millimeter) };
      duplicate.Children[0].Children[0].IQuantities.RemoveAt(0);

      Assert.True(original.Children[0].Children[0].B);
      Assert.Equal(1.0, original.Children[0].Children[0].D);
      Assert.Equal(1, original.Children[0].Children[0].I);
      Assert.Equal("a", original.Children[0].Children[0].S);
      Assert.Equal(TestEnum.Value1, original.Children[0].Children[0].TestEnum);
      Assert.Equal(quantity, original.Children[0].Children[0].IQuantity);
      Assert.Equal(force, original.Children[0].Children[0].Force);
      Assert.Equal(iQuantities, original.Children[0].Children[0].IQuantities);
      Assert.Equal(structs, original.Children[0].Children[0].Structs);
    }

    [Fact]
    public void EqualsTest1()
    {
      Force quantity = new Force(1, ForceUnit.Kilonewton);
      Force force = new Force(2, ForceUnit.Decanewton);
      IList<IQuantity> iQuantities = new List<IQuantity>() { Force.Zero, new Length(100, LengthUnit.Millimeter) };
      IList<Length> structs = new List<Length>() { Length.Zero, new Length(100, LengthUnit.Millimeter) };

      TestObject grandChild = new TestObject(true, 1.0, 1, "a", TestEnum.Value1, quantity, force, new List<TestObject>(), iQuantities, structs);
      TestObject original = new TestObject(new TestObject(grandChild));

      TestObject duplicate = (TestObject)original.Duplicate();

      Duplicates.AreEqual(original, duplicate);
    }

    public static IEnumerable<object[]> GetDataEqualsTest2()
    {
      var allData = new List<object[]>
        {
          new object[] {
            false,
            2.0,
            2,
            "b",
            TestEnum.Value2,
            new Force(1, ForceUnit.Kilonewton),
            new Force(2, ForceUnit.Decanewton),
            new List<IQuantity>() { Force.Zero, new Length(100, LengthUnit.Millimeter) },
            new List<Length>() { Length.Zero, new Length(100, LengthUnit.Millimeter) },
            new TestObject(false, 1.0, 1, "a", TestEnum.Value1, new Force(1, ForceUnit.Kilonewton), new Force(2, ForceUnit.Decanewton), new List<TestObject>(), new List<IQuantity>() { Length.Zero, new Length(100, LengthUnit.Millimeter) }, new List<Length>() { Length.Zero, new Length(100, LengthUnit.Millimeter) })
          },
      };
      return allData;
    }

    [Theory]
    [MemberData(nameof(GetDataEqualsTest2))]
    public void EqualsTest2(bool duplicateB, double duplicateD, int duplicateI, string duplicateS, TestEnum duplicateTestEnum, Force duplicateIQuantity, Force duplicateForce, IList<IQuantity> duplicateIQuantities, IList<Length> duplicateStructs, TestObject duplicateGrandChild)
    {
      Force quantity = new Force(1, ForceUnit.Kilonewton);
      Force force = new Force(2, ForceUnit.Decanewton);
      IList<IQuantity> iQuantities = new List<IQuantity>() { Force.Zero, new Length(100, LengthUnit.Millimeter) };
      IList<Length> structs = new List<Length>() { Length.Zero, new Length(100, LengthUnit.Millimeter) };

      TestObject grandChild = new TestObject(true, 1.0, 1, "a", TestEnum.Value1, quantity, force, new List<TestObject>(), iQuantities, structs);
      TestObject original = new TestObject(new TestObject(grandChild));
      TestObject duplicate = (TestObject)original.Duplicate();

      for (int i = 1; i <= 14; i++)
      {
        switch (i)
        {
          case 1:
            duplicate.Children[0].Children[0].B = duplicateB;
            break;
          case 2:
            duplicate.Children[0].Children[0].D = duplicateD;
            break;
          case 3:
            duplicate.Children[0].Children[0].I = duplicateI;
            break;
          case 4:
            duplicate.Children[0].Children[0].S = duplicateS;
            break;
          case 5:
            duplicate.Children[0].Children[0].TestEnum = duplicateTestEnum;
            break;
          case 6:
            duplicate.Children[0].Children[0].IQuantity = duplicateIQuantity;
            break;
          case 7:
            duplicate.Children[0].Children[0].Force = duplicateForce;
            break;
          case 8:
            duplicate.Children[0].Children[0].IQuantities = new List<IQuantity>();
            break;
          case 9:
            duplicate.Children[0].Children[0].IQuantities = duplicateIQuantities;
            break;
          case 10:
            duplicate.Children[0].Children[0].Structs = new List<Length>();
            break;
          case 11:
            duplicate.Children[0].Children[0].Structs = duplicateStructs;
            break;
          case 12:
            duplicate.Children[0].Children[0] = duplicateGrandChild;
            break;
          case 13:
            duplicate.Children[0].Children[0].Children = new List<TestObject>();
            break;
          case 14:
            duplicate.Children = new List<TestObject>();
            break;
        }
        //ObjectExtensionTest.Equals(original, duplicate);
        if (i < 14)
          Assert.Throws<Xunit.Sdk.EqualException>(() => Duplicates.AreEqual(original, duplicate));
        else
          Assert.Throws<Xunit.Sdk.TrueException>(() => Duplicates.AreEqual(original, duplicate));
      }
    }
  }

  public enum TestEnum
  {
    Value1 = 1,
    Value2 = 2,
    Value3 = 3,
    None = -1
  }

  public class TestObject
  {
    internal bool B { get; set; }
    internal double D { get; set; }
    internal int I { get; set; }
    internal string S { get; set; }
    internal TestEnum TestEnum { get; set; }
    internal IQuantity IQuantity { get; set; }
    internal Force Force { get; set; }
    internal IList<TestObject> Children { get; set; } = new List<TestObject>();
    internal IList<IQuantity> IQuantities { get; set; } = new List<IQuantity>();
    internal IList<Length> Structs { get; set; } = new List<Length>();

#pragma warning disable CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.
    public TestObject() { }

    public TestObject(TestObject child)
    {
      this.Children = new List<TestObject>() { child };
    }

    public TestObject(IList<TestObject> children)
    {
      this.Children = children;
    }
#pragma warning restore CS8618 // Non-nullable field must contain a non-null value when exiting constructor. Consider declaring as nullable.

    internal TestObject(bool b, double d, int i, string s, TestEnum testEnum, IQuantity quantity, Force force, IList<TestObject> children, IList<IQuantity> iQuantities, IList<Length> structs)
    {
      this.B = b;
      this.D = d;
      this.I = i;
      this.S = s;
      this.TestEnum = testEnum;
      this.IQuantity = quantity;
      this.Force = force;
      this.Children = children;
      this.IQuantities = iQuantities;
      this.Structs = structs;
    }
  }
}
