using UnityEngine.EventSystems;

public class BabyBee : Bee {

    public bool isGrowing;
    public bool isQueen;
    
    // Flag to manage age of baby bee
    public bool isEgg;

    protected override void Start() {
        base.Start();
        isEgg = true;
    }

    protected override void OnMouseDown() {
        if (isGrowing) {
            // Make sure UI isn't on UI or in main menu
            if (inMenu || EventSystem.current.IsPointerOverGameObject())
                return;
            profileController.Hide();
            babyProfileController.Show(this);
        }
        else {
            hudController.eggMenu.Show(this);
        }
    }
}
