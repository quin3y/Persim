using UnityEngine;
using System;
using System.Collections.Generic;

namespace UnityStandardAssets.Characters.ThirdPerson
{
	public class SimulationEngine : MonoBehaviour {
		AICharacterControl characterController;			// AI Character controller attached on the character
		StateSpaceManager stateSpaceManager;			// state space manager attached on the character
		ContextDrivenSimulation contextDrvSimulation;
		SimulationEntity simEntity;

		void Start() {
			// configure simulation
			SimulationEntity.ReadObjectXml();							// static method: read objects info 

			simEntity = new SimulationEntity();
			simEntity.Actions = Utils.ReadActionXml(this.name);			// TODO ReadActionXml with static Actions
			simEntity.Activities = Utils.ReadActivityXml(this.name);	// TODO ReadActivityXml with static Activities
			simEntity.ReadContextXml(this.name);

			// TODO AICharacterControl is located on another character
			//characterController = GameObject.Find("Ethan").GetComponent<AICharacterControl>();	
			characterController = GameObject.FindGameObjectWithTag("Character").GetComponent<AICharacterControl>();

			// initialize state space
			// TODO StateSpaceManager is located on another GameObject
			stateSpaceManager = GameObject.Find("Camera").GetComponent<StateSpaceManager>();	
			stateSpaceManager.InitializeStateSpace();

			contextDrvSimulation = new ContextDrivenSimulation (characterController, stateSpaceManager, simEntity);

		}

		void Update() { 			
			if (characterController.activityFinished) {
				characterController.activityFinished = false;
				TransitionContext ();
			}
		}
			
		// run simulation loop: WITHIN a context
		public void RunSimulation() {
			// Run context-driven simulation engine
			contextDrvSimulation.SelectContextActivities ();
			contextDrvSimulation.ScheduleContextActivities ();
			contextDrvSimulation.PerformContextActivity ();
			//contextDrvSimulation.EvaluateStateSpace ();			// moved into TransitionContext()
			//contextDrvSimulation.TransitToNextContext ();			// moved into TransitionContext()
		}

		// transition to the next context if available: BETWEEN contexts
		void TransitionContext() {
			print ("Now " + stateSpaceManager.StateSpaceHistory.Count + " state spaces has stored");
			contextDrvSimulation.EvaluateStateSpace ();
			contextDrvSimulation.TransitToNextContext ();
			if (!simEntity.IsEnd ())
				RunSimulation ();
			else
				Debug.Log ("Simulation Ends");			
		}

		public SimulationEntity SimEntity {
			get { return  simEntity; }
			set { simEntity = value; }
		}
	}
}

