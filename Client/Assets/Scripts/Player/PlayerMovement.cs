using Interactable;
using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.AI;


namespace Player
{
    public class PlayerMovement : MonoBehaviour
    {

        //target to fallow
        private Transform target;

        //reference to our agent
        public NavMeshAgent agent;

        //reference to our camera
        private Camera cam;

        //movementMask is a Layer that we define as Ground on Unity
        public LayerMask movementMask;

        //interctable control
        private InteractableObject focus;




        // Start is called before the first frame update
        private void Start()
        {
            cam = Camera.main;
            agent = GetComponent<NavMeshAgent>();

        }
    

        // Update is called once per frame
        private void Update()
        {

            //Here we are verifying if the mouse point is over a GameObject
            //in other word, we are verifyin if the point mouse is over the UI components such as Image, Panel and things like that.         
            if (EventSystem.current.IsPointerOverGameObject())
            {
                //if mouse over a UI component, just ignore the rest of the Update() code
                return;
            }


            //Mouse-Left Behavior
            if (Input.GetMouseButtonDown(0))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100, movementMask)) //if movementMask layer is Ground and the place where you clicked also is Ground, then enter the if
                {
                    //move our player to what we hit
                    MoveToPoint(hit.point);

                    //stop focosing any objects
                    RemoveFocus();
                }
            }

            //Mouse-Right Behavior
            if (Input.GetMouseButtonDown(1))
            {
                Ray ray = cam.ScreenPointToRay(Input.mousePosition);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit, 100))
                {
                    //check if we hit an interactable (object with Interactable component)
                    ////if we did set it as our focus
                    ///
                    InteractableObject interactableObject = hit.collider.GetComponent<InteractableObject>(); //get InteractableObject component

                    //if InteractableObject is not null, in other word if we clicked on GameObject that have InteractableObject Scrpt, so get in the if
                    //and setFocus, 
                    if (interactableObject != null)
                    {
                        SetFocus(interactableObject);
                    }
                }
            }



            if (target != null)
            {
                //if target exist, so go up to the target
                //agent.SetDestination(target.position);
                MoveToPoint(target.position);

                //look at GameObject target
                FaceTarget();
            }

            //TODO send position to server
            //GameManager.players[Client.instance.myId].transform.position = agent.transform.position;
            //GameManager.players[Client.instance.myId].transform.rotation = agent.transform.rotation;
            //ClientSend.PlayerMovement();
        }
    

        //method to move our agent/player to the position we pass as parameter
        private void MoveToPoint(Vector3 _point)
        {
            ClientSend.MoveTo(_point);

            //agent.SetDestination(point);
            
        }

        //method
        private void FollowTarget(InteractableObject newTarget)
        {
            agent.stoppingDistance = newTarget.radius * .8f;
            agent.updateRotation = false;
            target = newTarget.interactionTransform;
        }


        //method
        private void StopFollowingTarget()
        {
            agent.stoppingDistance = 0f;
            agent.updateRotation = true;
            target = null;
        }


        //method
        private void RemoveFocus()
        {
            if (focus != null)
            {
                focus.OnDefocused();
            }

            focus = null;
            StopFollowingTarget();
        }


        //method
        private void SetFocus(InteractableObject newFocus)
        {
            if (newFocus != focus)
            {
                if (focus != null)
                {
                    focus.OnDefocused();
                }

                focus = newFocus;
                FollowTarget(newFocus);

            }


            newFocus.OnFocused(transform);

        }

        private void FaceTarget()
        {
            Vector3 direction = (target.position - transform.position).normalized;
            Quaternion lookRotation = Quaternion.LookRotation(new Vector3(direction.x, 0f, direction.z));
            transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 5f);
        }




    }

}
