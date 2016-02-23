using UnityEngine;
using System;

public class ActionInfo
{
	public string name {get; set;}
	public int id {get; set;}
	public Vector3 location {get; set;}
	public string obj {get; set;}


    public ActionInfo(string name, int id, Vector3 loc, string obj) {
		this.name = name;
        this.id = id;
        this.location = loc;
		this.obj = obj;
    }
	
}

