using System.Collections.Generic;
using GH_IO.Serialization;

namespace OasysGH.Helpers {
  internal class DeSerialization {
    internal static void ReadDropDownComponents(ref GH_IReader reader, ref List<List<string>> dropDownItems, ref List<string> selecteditems, ref List<string> spacerDescriptions) {
      // skip reading anything if dropdown hasnt been set by write method
      if (reader.ItemExists("dropdown")) {
        // dropdown content list
        if (reader.GetBoolean("dropdown")) {
          int dropdownCount = reader.GetInt32("dropdownCount");
          dropDownItems = new List<List<string>>();
          for (int i = 0; i < dropdownCount; i++) {
            int dropDownContentsCount = reader.GetInt32("dropdowncontentsCount" + i);
            var tempContent = new List<string>();
            for (int j = 0; j < dropDownContentsCount; j++)
              tempContent.Add(reader.GetString("dropdowncontents" + i + j));
            dropDownItems.Add(tempContent);
          }
        }

        // spacer list
        if (reader.GetBoolean("spacer")) {
          int dropDownSpacerCount = reader.GetInt32("spacerCount");
          spacerDescriptions = new List<string>();
          for (int i = 0; i < dropDownSpacerCount; i++)
            spacerDescriptions.Add(reader.GetString("spacercontents" + i));
        }

        // selection list
        if (reader.GetBoolean("select")) {
          int selectionsCount = reader.GetInt32("selectionCount");
          selecteditems = new List<string>();
          for (int i = 0; i < selectionsCount; i++)
            selecteditems.Add(reader.GetString("selectioncontents" + i));
        }
      }
    }

    internal static GH_IWriter WriteDropDownComponents(ref GH_IWriter writer, List<List<string>> dropDownItems, List<string> selectedItems, List<string> spacerDescriptions) {
      // to save the dropdownlist content, spacer list and selection list
      // loop through the lists and save number of lists as well
      bool dropDown = false;
      if (dropDownItems != null) {
        writer.SetInt32("dropdownCount", dropDownItems.Count);
        for (int i = 0; i < dropDownItems.Count; i++) {
          writer.SetInt32("dropdowncontentsCount" + i, dropDownItems[i].Count);
          for (int j = 0; j < dropDownItems[i].Count; j++)
            writer.SetString("dropdowncontents" + i + j, dropDownItems[i][j]);
        }

        dropDown = true;
      }

      writer.SetBoolean("dropdown", dropDown);

      // spacer list
      bool spacer = false;
      if (spacerDescriptions != null) {
        writer.SetInt32("spacerCount", spacerDescriptions.Count);
        for (int i = 0; i < spacerDescriptions.Count; i++)
          writer.SetString("spacercontents" + i, spacerDescriptions[i]);
        spacer = true;
      }

      writer.SetBoolean("spacer", spacer);

      // selection list
      bool select = false;
      if (selectedItems != null) {
        writer.SetInt32("selectionCount", selectedItems.Count);
        for (int i = 0; i < selectedItems.Count; i++)
          writer.SetString("selectioncontents" + i, selectedItems[i]);
        select = true;
      }

      writer.SetBoolean("select", select);

      return writer;
    }
  }
}
