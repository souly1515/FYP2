using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Venus_Main : E_Base{
	public enum InternalAttackStates
	{
		IDLE,
		DIG,
		ASSAULT_BITE_PREP,
		ASSAULT_BITE,
		EMERGE,
		EMERGE_PREP,
		SPAWN_PODS,
		SPIKE_PREP,//for from body spike attack
		SPIKE,
		GROUND_SPIKE_PREP,//for the ground spike attack like the lion
		GROUND_SPIKE
	}
	List<GameObject> attackList;
	public InternalAttackStates a_states=InternalAttackStates.IDLE;
	bool UnderGround=false;
	GameObject Flytrap;
	SpriteRenderer s_render;
	public GameObject SpikePrefab;
	public GameObject SporePodPrefab;
	public GameObject BiterPrefab;//for the assault biting attack
	public int podLeft = 0;//pods left to spawn

	public float restTime=3.0f;
	public float podSpawnTime = 0.5f;
	public float spikePrepTime = 1.0f;
	public float digTime=1.0f;
	public float surfaceTime=1.0f;

	protected override void Start ()
	{
		attackList = new List<GameObject> ();
		Transform temp = transform.FindChild ("FlyTrap");
		if (temp)
			Flytrap = temp.gameObject;
		s_render = GetComponent<SpriteRenderer> ();
		base.Start ();
	}

	void Dig(bool dig)
	{
		if (dig) {
			UnderGround = true;
			if (Flytrap)
				Flytrap.SetActive (false);
			if (s_render)
				s_render.enabled = false;
			//set collider inactive here
		} else {
			UnderGround = false;
			if (Flytrap)
				Flytrap.SetActive (true);
			if (s_render)
				s_render.enabled = true;
			//set collider active here
		}
	}

	protected override void Attack_State ()
	{
		timeLeft -= Time.deltaTime;
		switch (a_states) {
		case InternalAttackStates.IDLE:
			if(timeLeft<=0)
			{
				int t=Random.Range(0,5);
				switch(t)
				{
				case 0://assault bite
					a_states=InternalAttackStates.ASSAULT_BITE_PREP;
					//first dig into the ground
					//then spawn the bitters
					//once done then emerge and eat from the ground

					break;
				case 1://spawn pods
					a_states=InternalAttackStates.SPAWN_PODS;
					timeLeft=podSpawnTime;
					//create the pods...
					//done (LOL)
					break;
				case 2://ground spike
					//create the spikes...
					//done (lol?)
					break;
				case 3://spike
					a_states=InternalAttackStates.SPIKE_PREP;
					timeLeft=spikePrepTime;
					//wait for prep to be over
					//then spawn several spikes to hit the player

					break;
				case 4://dig and eat
					a_states=InternalAttackStates.DIG;
					timeLeft=digTime;
					//go into the ground
					//and emerge from the ground

					break;
				}
			}
			break;
		case InternalAttackStates.DIG:
			//disable my collider
			//disable my sprite renderer after this as well as most child objects
			Dig (true);
			timeLeft=digTime;
			a_states=InternalAttackStates.EMERGE_PREP;
			//set up time to remerge
			break;
		case InternalAttackStates.EMERGE_PREP:
			if(timeLeft<=0)//wait for a while if nessasary
			{
				//activate dust thing;
				//in x sec change state to emerge
				timeLeft=surfaceTime;
			}
			break;
		case InternalAttackStates.EMERGE:
			if(timeLeft<=0)
			{
				Dig(false);
				//deactivate dust thing
				//deal damage to whatever is in my collider box thing
				//if animation played finish
				a_states=InternalAttackStates.IDLE;
			}
			break;
		case InternalAttackStates.ASSAULT_BITE_PREP:
			if(!UnderGround)
			{
				Dig (true);
				//set time to attack
				timeLeft=1.0f;
			}
			if(timeLeft<=0)
			{
				timeLeft=0.5f;
				//spawn attack object
				//place it into the attacklist
				a_states=InternalAttackStates.ASSAULT_BITE;
			}
			break;
		case InternalAttackStates.ASSAULT_BITE:
			if(timeLeft<=0)
			{
				//if not triggered attackobject
				//set trigger on attack object
				//else
				//wait for attack object to despawn
				//if it despawned set state to emerge
				a_states=InternalAttackStates.EMERGE_PREP;
				timeLeft=digTime;
			}
			break;
		case InternalAttackStates.SPAWN_PODS:
			if(podLeft<=0)
			{
				podLeft=Random.Range(3,6);
			}
			else
			{
				if(timeLeft<=0)
				{
					podLeft-=1;

					//spawn pods
					timeLeft=podSpawnTime;
					if(podLeft<=0)
					{
						a_states=InternalAttackStates.IDLE;
						timeLeft=restTime;
					}
				}
			}
			break;
		case InternalAttackStates.SPIKE_PREP:
			if(timeLeft<=0)
			{
				a_states=InternalAttackStates.SPIKE;
			}
			break;
		case InternalAttackStates.SPIKE:
		{
			bool b=false;
			foreach(GameObject g in attackList)
			{
				if(g)
				{
					b=true;
					break;
				}
			}
			if(!b)
			{
				//set animation back to idle
				a_states=InternalAttackStates.IDLE;
				timeLeft=restTime;
			}
		}	
			break;
		case InternalAttackStates.GROUND_SPIKE_PREP:
			//lift from lion code
			break;
		case InternalAttackStates.GROUND_SPIKE:
			//lift from lion code
			break;
		}
	}

	protected override void Idle_State ()
	{

	}

	public override void KnockBack (float amount, Vector2 Dir, float stunDuration)
	{
	}

	protected override void Tracking_State ()
	{

	}

	protected override void ChangeState ()
	{
		states = E_States.ATTACK;
	}
	
}
