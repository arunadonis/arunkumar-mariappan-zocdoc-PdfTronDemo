using System;
using System.Collections.Generic;
using System.IO;

// Most commonly used namespaces for PDFTron SDK.
using pdftron;
using pdftron.Common;
using pdftron.FDF;
using pdftron.PDF;
using pdftron.PDF.Annots;
using pdftron.SDF;

namespace MyApp
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            // Initialize PDFNet before using any PDFTron related
            // classes and methods (some exceptions can be found in API)
            PDFNet.Initialize("demo:1641996103252:7b2db4fa0300000000aaa6f395741c96ee6edf862867eb5ae48ea28c07");
            // The vector used to store the name and count of all fields.
            // This is used later on to clone the fields
            Dictionary<string, int> field_names = new Dictionary<string, int>();

            //----------------------------------------------------------------------------------
            // Scenario 1: Programatically create new Form Fields and Widget Annotations.
            //----------------------------------------------------------------------------------

            // Using PDFNet related classes and methods, must  
            // catch or throw PDFNetException
            try
            {

                string filename = "DemographicForm.pdf";
                // Create a blank page
                PDFDoc doc = new PDFDoc(filename);
                Page page = doc.GetPage(1);

                // Text Widget Creation 
                // Create an empty text widget with black text.
                TextWidget text1 = TextWidget.Create(doc, new Rect(80, 700, 380, 720), "patientName");                
                text1.SetText("Patient Name");
                text1.RefreshAppearance();
                page.AnnotPushFront(text1);

                // RadioButton Widget Creation
                // Create a radio button group and add three radio buttons in it. 					
                RadioButtonGroup radio_group = RadioButtonGroup.Create(doc, "RadioGroup");
                RadioButtonWidget radiobutton1 = radio_group.Add(new Rect(142, 619, 152, 629));
                radiobutton1.SetBackgroundColor(new ColorPt(1, 1, 1), 3);
                radiobutton1.RefreshAppearance();
                RadioButtonWidget radiobutton2 = radio_group.Add(new Rect(214, 619, 224, 629));
                radiobutton2.SetBackgroundColor(new ColorPt(1, 1, 1), 3);
                // Enable the single radio button. By default the first one is selected
                radiobutton2.EnableButton();
                radiobutton2.SetBackgroundColor(new ColorPt(1, 1, 1), 3);
                radiobutton2.RefreshAppearance();
                RadioButtonWidget radiobutton3 = radio_group.Add(new Rect(286, 619, 296, 629));                
                radiobutton3.RefreshAppearance();
                RadioButtonWidget radiobutton4 = radio_group.Add(new Rect(358, 619, 368, 629));
                radiobutton4.SetBackgroundColor(new ColorPt(1, 1, 1), 3);
                radiobutton4.RefreshAppearance();
                RadioButtonWidget radiobutton5 = radio_group.Add(new Rect(430, 619, 440, 629));
                radiobutton5.SetBackgroundColor(new ColorPt(1, 1, 1), 3);
                radiobutton5.RefreshAppearance();
                RadioButtonWidget radiobutton6 = radio_group.Add(new Rect(502, 619, 512, 629));
                radiobutton6.SetBackgroundColor(new ColorPt(1, 1, 1), 3);
                radiobutton6.RefreshAppearance();
                radio_group.AddGroupButtonsToPage(page);

                doc.Save("TestForm.pdf", SDFDoc.SaveOptions.e_linearized);
                
            }
            catch (PDFNetException e)
            {
                System.Console.WriteLine(e.Message);
            }

            //----------------------------------------------------------------------------------
            // Scenario 2: 
            // Fill-in forms / Modify values of existing fields.
            // Traverse all form fields in the document (and print out their names). 
            // Search for specific fields in the document.
            //----------------------------------------------------------------------------------
            try
            {
                using (PDFDoc doc = new PDFDoc("TestForm.pdf"))
                {
                    doc.InitSecurityHandler();

                    FieldIterator itr;
                    for (itr = doc.GetFieldIterator(); itr.HasNext(); itr.Next()) {
                        Field field = itr.Current();

                        Console.WriteLine("Field name: {0}", field.GetName());
                    }

                    // Search for a specific field
                    Field fld = doc.GetField("patientName");
                    if (fld != null) {
                        Console.WriteLine("Field search for {0} was successful", fld.GetName());
                    } else {
                        Console.WriteLine("Field search failed.");
                    }
                    fld.SetValue("John Doe");
                    // Regenerate field appearances.
                    doc.RefreshFieldAppearances();

                    // Search for a specific field
                    Field fld1 = doc.GetField("RadioGroup");
                    if (fld1 != null) {
                        Console.WriteLine("Field search for {0} was successful", fld1.GetName());
                    } else {
                        Console.WriteLine("Field search failed.");
                    }
                    fld1.SetValue(true);
                    // Regenerate field appearances.
                    doc.RefreshFieldAppearances();
                    doc.Save("TestForm_Filled.pdf", SDFDoc.SaveOptions.e_linearized);
                    Console.WriteLine("Done.");
                }
            }
            catch (PDFNetException e)
            {
                Console.WriteLine(e.Message);
            }


            //----------------------------------------------------------------------------------
            // Scenario 3: 
            // Flatten all form fields in a document.
            //----------------------------------------------------------------------------------
            try
            {
                using (PDFDoc doc = new PDFDoc("TestForm_Filled.pdf"))
                {
                    doc.InitSecurityHandler();
                    doc.FlattenAnnotations();
                    doc.Save("TestForm_Flattened.pdf", SDFDoc.SaveOptions.e_linearized);
                    Console.WriteLine("Done.");
                }
            }
            catch (PDFNetException e)
            {
                Console.WriteLine(e.Message);
            }


            // Makes the program wait to terminate after user input
            Console.ReadKey();
        }
    }
}