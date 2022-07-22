using System;
using System.Collections;
using System.Collections.Generic;
using System.IO.MemoryMappedFiles;
using System.Text;
using System.Threading;
using UnityEngine;

class MemorySharePython
{
    //���������������ڷ�������գ����û�����Ϊ10
    private MemoryMappedFile mmfsend;
    private MemoryMappedViewAccessor viewAccessorSend;

    private int memorysize;
    private string Sendfilename;
    private string Recvicefilename;

    public MemorySharePython( int memorysize = 4096)
    {
      
        this.memorysize = memorysize;

        //����һ���ڴ�����ڹ���
        

    }


    public void WriteMemory(string s,string Sendfilename)
    {
        s = s + "@";
        mmfsend = MemoryMappedFile.CreateOrOpen(Sendfilename, memorysize, MemoryMappedFileAccess.ReadWrite);
        viewAccessorSend = mmfsend.CreateViewAccessor(0, memorysize);
        viewAccessorSend.Write(0, memorysize);
        viewAccessorSend.WriteArray<byte>(0, System.Text.Encoding.ASCII.GetBytes(s), 0, s.Length);

    }


    public string ReadMemory(string Recvicefilename)
    {



        try
        {
            MemoryMappedFile mmf = MemoryMappedFile.OpenExisting(Recvicefilename);
            MemoryMappedViewAccessor viewAccessor = mmf.CreateViewAccessor(0, memorysize);
            byte[] charsInMMf = new byte[memorysize];
            viewAccessor.ReadArray<byte>(0, charsInMMf, 0, memorysize);
            string msg = Encoding.ASCII.GetString(charsInMMf);
            if (msg.Contains("}@"))
            {
                int index = msg.IndexOf("}@");
                msg = msg.Substring(0, index + 1);
            }
            return msg;

        }
        catch (Exception e)
        {
            Debug.Log(e);
            return null;
        }






    }





}