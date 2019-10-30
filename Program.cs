using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xbim.Ifc2x3.Kernel;
using Xbim.Ifc2x3.ProductExtension;
using Xbim.IO;
using Xbim.ModelGeometry;
using Xbim.Ifc;
using Xbim.Ifc4.Interfaces;

namespace Console_IFC
{
    class Program
    {
        static void Main(string[] args)
        {
            const string fileName = "C:\\Users\\henrikn\\BundeGruppen AS\\BIM utvikling - General\\Dummy filer\\Dummy_Model_ps.ifc";
            using (var model = IfcStore.Open(fileName))
            {
                //get all doors in the model (using IFC4 interface of IfcDoor this will work both for IFC2x3 and IFC4)
                var allDoors = model.Instances.OfType<IIfcDoor>();

                //get only doors with defined IIfcTypeObject
                var someDoors = model.Instances.Where<IIfcDoor>(d => d.IsTypedBy.Any());

                //get all walls in the model
                var walls = model.Instances.OfType<IIfcWall>();

                //iterate over all the walls and change them
                foreach (var wall in walls)
                    Console.WriteLine(wall.Name);
                // wall.Name = "Iterated wall: " + wall.Name;

                //get all spaces in the model
                var spaces = model.Instances.OfType<IIfcSpace>();
                foreach (var space in spaces)
                    Console.WriteLine(space.LongName + " - " + space.Name);
                    

                //get one single door 
                var id = "3OAbz$kW1DyuZY2KLwUznB";
                var theDoor = model.Instances.FirstOrDefault<IIfcDoor>(d => d.GlobalId == id);
                Console.WriteLine($"Door ID: {theDoor.GlobalId}, Name: {theDoor.Name}");

                //get all single-value properties of the door
                var properties = theDoor.IsDefinedBy
                    .Where(r => r.RelatingPropertyDefinition is IIfcPropertySet)
                    .SelectMany(r => ((IIfcPropertySet)r.RelatingPropertyDefinition).HasProperties)
                    .OfType<IIfcPropertySingleValue>();
                foreach (var property in properties)
                    Console.WriteLine($"Property: {property.Name}, Value: {property.NominalValue}");
                Console.ReadLine();
            }
        }
    }
}
