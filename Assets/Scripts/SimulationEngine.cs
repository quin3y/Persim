using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class SimulationEngine : MonoBehaviour {
		AICharacterControl characterController;

		void Start() {
			characterController = GameObject.Find("Ethan").GetComponent<AICharacterControl>();

			SimulationEntity.Objects = Utils.ReadObjectXml();
			SimulationEntity.Actions = Utils.ReadActionXml();
			SimulationEntity.Activities = Utils.ReadActivityXml();
			SimulationEntity.ReadContextXml();


			RunSimulation ();
		}

		void Update() { }
			
		public SimulationEngine() { }

		// Run simulation engine
		void RunSimulation() {
			// Run context-driven simulation engine
			ContextDrivenSimulation contextDSimulation = new ContextDrivenSimulation (characterController);
			contextDSimulation.RunSimulationLoop ();
		}
	}
}

