using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class SimulationEngine : MonoBehaviour {
		AICharacterControl characterController;			// AI Character controller attached on the character
		StateSpaceManager stateSpaceManager;			// state space manager attached on the character

		void Start() {
			// configure simulation
			SimulationEntity.ReadObjectXml();
			SimulationEntity.Actions = Utils.ReadActionXml();	// TODO ReadActionXml with static Actions
			SimulationEntity.Activities = Utils.ReadActivityXml();	// TODO ReadActivityXml with static Activities
			SimulationEntity.ReadContextXml();

			// TODO AICharacterControl is located on another character
			characterController = GameObject.Find("Ethan").GetComponent<AICharacterControl>();	

			// initialize state space
			// TODO StateSpaceManager is located on another GameObject
			stateSpaceManager = GameObject.Find("Camera").GetComponent<StateSpaceManager>();	
			stateSpaceManager.InitializeStateSpace();

			//RunSimulation ();
		}

		void Update() { 
			
		}
			
		public SimulationEngine() { }

		// Run simulation loop
		void RunSimulation() {
			// Run context-driven simulation engine
			ContextDrivenSimulation cds = new ContextDrivenSimulation (characterController, stateSpaceManager);
			cds.SelectContextActivities ();
			cds.ScheduleContextActivities ();
			cds.PerformContextActivity ();
//			cds.EvaluateStateSpace ();
//			cds.TransitToNextContext ();

		}
	}
}

