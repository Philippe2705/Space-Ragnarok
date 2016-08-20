using UnityEngine;

public class BotShip : Ship
{
    float mouveHorizontal;
    float mouveVertical;
    float hasAlreadyTurn;
    bool playTimer;
    float timer;

    protected override void UpdateServer()
    {
        base.UpdateServer();


        if (transform.localPosition.x >= 50)
        {
            if (transform.localRotation.eulerAngles.z >= 270 || transform.localRotation.eulerAngles.z <= 90)
            {
                //tourne a gauche
                mouveHorizontal = -1;
                hasAlreadyTurn = 4;
            }
            else
            {
                //tourne a droite
                mouveHorizontal = 1;
                hasAlreadyTurn = 4;
            }
        }
        else if (transform.localPosition.x <= -50)
        {

            if (transform.localRotation.eulerAngles.z <= 90 || transform.localRotation.eulerAngles.z >= 270)
            {
                //tourne a gauche
                mouveHorizontal = 1;
                hasAlreadyTurn = 4;
            }
            else
            {
                //tourne a droite
                mouveHorizontal = -1;
                hasAlreadyTurn = 4;
            }
        }
        else if (transform.localPosition.y >= 20)
        {

            if (transform.localRotation.eulerAngles.z >= 0)
            {
                //tourne a gauche
                mouveHorizontal = -1;
                hasAlreadyTurn = 4;
            }
            else
            {
                //tourne a droite
                mouveHorizontal = 1;
                hasAlreadyTurn = 4;
            }
        }
        else if (transform.localPosition.y <= -20)
        {

            if (transform.localRotation.eulerAngles.z >= 180)
            {
                //tourne a gauche
                mouveHorizontal = -1;
                hasAlreadyTurn = 4;
            }
            else
            {
                //tourne a droite
                mouveHorizontal = 1;
                hasAlreadyTurn = 4;
            }
        }
        else if (hasAlreadyTurn <= 0)
        {
            RandomMove();
            hasAlreadyTurn = 4;
        }
        else
        {
            hasAlreadyTurn -= Time.deltaTime;
            if (!playTimer)
            {
                mouveHorizontal = 0;
            }
        }

        //CmdFire(fireSide);
        CmdMove(mouveVertical, mouveHorizontal);
    }
    void RandomMove()
    {
        float nombre = Random.Range(-1f, 1f);
        if (nombre >= 0)
        {
            mouveHorizontal = 1;
        }
        else
        {
            mouveHorizontal = -1;
        }
        playTimer = true;
        timer = Random.Range(1f, 2f);
    }
}
