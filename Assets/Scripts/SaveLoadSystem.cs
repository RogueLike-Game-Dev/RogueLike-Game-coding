using System;
using System.Collections.Generic;
using UnityEngine;
using  System.IO;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveLoadSystem
{
        private static string runsFolderPath = Application.persistentDataPath + "/runs";
        private static int nrRun = 0;
        
        public static void SaveRun() //save a run in a different binary file
        {
                
                BinaryFormatter formatter = new BinaryFormatter();
                if (!Directory.Exists(runsFolderPath))
                {
                        Directory.CreateDirectory(runsFolderPath);
                }

                if (RunStats.selectedSlot ==
                    null) //if user didn't select a slot, we should verify if we have 5 files saved already
                {
                        //and we should ovveride the older one
                        //otherwise if we don't have 5 files we should create one new

                        int noFilesSaved = 0;
                        foreach (string _ in Directory.GetFiles(runsFolderPath)) //verify number of saved files
                        {
                                noFilesSaved++;
                        }

                        if (noFilesSaved < 5) //if there are less than 5 saved files, create a new one
                        {
                                Debug.Log("No runs saved create a new file");
                                FileStream stream = new FileStream(runsFolderPath + "/" + "SaveRun" + (noFilesSaved +1), FileMode.Append);
                                SaveData data = new SaveData();
                                formatter.Serialize(stream, data);
                                stream.Close();
                        }
                        else { //if there are 5 files saved, delete and replace the older one
                                
                                string olderFileName = "";
                                DateTime firstUpdated = DateTime.MaxValue;
                                foreach (FileInfo file in new DirectoryInfo(runsFolderPath).GetFiles())
                                {
                                        if (file.LastWriteTime < firstUpdated)
                                        {
                                                firstUpdated = file.LastWriteTime;
                                                olderFileName = file.Name;

                                        }
                                }

                                if (olderFileName != "")
                                {
                                       File.Delete(runsFolderPath+ "/" +olderFileName); 
                                       FileStream stream = new FileStream(runsFolderPath+ "/" +olderFileName, FileMode.Append);
                                       SaveData data = new SaveData();
                                       formatter.Serialize(stream, data);
                                       stream.Close();
                                }
                                
                        }
                }
                else //if user selected a save slot we should override it
                {
                        File.Delete(runsFolderPath+ "/" +RunStats.selectedSlot); 
                        FileStream stream = new FileStream(runsFolderPath+ "/" +RunStats.selectedSlot, FileMode.Append);
                        SaveData data = new SaveData();
                        formatter.Serialize(stream, data);
                        stream.Close();
                        
                }

        }

        public static List<SaveData> LoadRuns()
        {
                List <SaveData> savedRuns = new List<SaveData>();
                foreach (string fileName in Directory.GetFiles(runsFolderPath))
                {
                        BinaryFormatter formatter = new BinaryFormatter();
                        FileStream stream = new FileStream(fileName, FileMode.Open);
                        SaveData data = formatter.Deserialize(stream) as SaveData;
                        savedRuns.Add(data);
                        stream.Close();
                }

                return savedRuns;

        }

        public static SaveData LoadRun(string selectedSlot)
        {

                if (File.Exists(runsFolderPath + "/" + selectedSlot))
                {
                        BinaryFormatter formatter = new BinaryFormatter();
                        FileStream stream = new FileStream(runsFolderPath + "/" + selectedSlot, FileMode.Open);
                        SaveData data = formatter.Deserialize(stream) as SaveData;
                        stream.Close();
                        return data;
                }

                return null;

        }
        
}
