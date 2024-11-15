using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BookMovement : MonoBehaviour
{
    [System.Serializable]
    public class BookData
    {
        [SerializeField]
        private Transform book;    // Referencia al libro
        [SerializeField]
        private List<Transform> waypoints;  // Lista de puntos
        [SerializeField]
        private float speed = 5f;  // Velocidad individual
        [SerializeField]
        private float rotationSpeed = 100f; // Velocidad de rotación
        [SerializeField]
        private Vector3 rotationAxis = Vector3.forward; // Eje de rotación

        private int currentWaypointIndex = 0; // Índice del punto actual
        private Vector3 target;    // Posición objetivo

        // Getters y Setters
        public Transform Book { get => book; set => book = value; }
        public List<Transform> Waypoints { get => waypoints; set => waypoints = value; }
        public float Speed { get => speed; set => speed = value; }
        public float RotationSpeed { get => rotationSpeed; set => rotationSpeed = value; }
        public Vector3 RotationAxis { get => rotationAxis; set => rotationAxis = value; }
        public Vector3 Target { get => target; set => target = value; }
        public int CurrentWaypointIndex { get => currentWaypointIndex; set => currentWaypointIndex = value; }
    }

    [SerializeField]
    private List<BookData> books = new List<BookData>(); // Lista de libros y sus datos
    private bool isPlayerInZone = false;
    private Coroutine moveCoroutine;

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = true;
            if (moveCoroutine == null)
            {
                moveCoroutine = StartCoroutine(MoveBooks());
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerInZone = false;
            if (moveCoroutine != null)
            {
                StopCoroutine(moveCoroutine);
                moveCoroutine = null;
            }
        }
    }

    IEnumerator MoveBooks()
    {
        // Inicializar los objetivos para cada libro asumiendo el primer waypoint como destino
        foreach (BookData bookData in books)
        {
            if (bookData.Waypoints != null && bookData.Waypoints.Count > 0)
            {
                bookData.CurrentWaypointIndex = 0;
                bookData.Target = bookData.Waypoints[bookData.CurrentWaypointIndex].position;
            }
            else
            {
                Debug.LogWarning("El libro " + bookData.Book.name + " no tiene waypoints asignados.");
            }
        }

        while (isPlayerInZone)
        {
            foreach (BookData bookData in books)
            {
                if (bookData.Waypoints == null || bookData.Waypoints.Count == 0)
                    continue; // Saltar si no hay waypoints

                // Mover cada libro hacia su punto objetivo
                if (Vector3.Distance(bookData.Book.position, bookData.Target) > 0.01f)
                {
                    float step = bookData.Speed * Time.deltaTime;
                    bookData.Book.position = Vector3.MoveTowards(bookData.Book.position, bookData.Target, step);

                    // Rotar el libro mientras se mueve
                    bookData.Book.Rotate(bookData.RotationAxis, bookData.RotationSpeed * Time.deltaTime, Space.Self);
                }
                else
                {
                    // Avanzar al siguiente waypoint
                    bookData.CurrentWaypointIndex = (bookData.CurrentWaypointIndex + 1) % bookData.Waypoints.Count;
                    bookData.Target = bookData.Waypoints[bookData.CurrentWaypointIndex].position;
                }
            }
            yield return null;
        }
    }
}
