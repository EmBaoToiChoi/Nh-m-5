[System.Serializable]
public class GameData
{
    public float playerX;
    public float playerY;

    public float currentHealth;
    public float currentEnergy;

    public int currentXP;
    public int currentLevel;
    public int coin;

    // Gun
    public int currentAmmo;
    public int reserveAmmo;
    public int maxAmmo;

    // Bow
    public int bowAmmo;

    // Vũ khí
    public bool hasGun;
    public bool hasBow;

    public string sceneName;
    public int soBinhMau;
}
