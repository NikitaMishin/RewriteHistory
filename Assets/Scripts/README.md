
Существующие скрипты:

BezierCurveMovement 
BezierCurveMovementWithRewind
RigidBodyRewind
SimpleRewind
BezierCurvePlayerController
ManagerController
OrdinaryAndBezierCheckoutTrigger
OrdinaryPlayerController
TimeController


1) Накидываем TimeController как синглтон на какой-нибудь обеъект на сцене, который никогда ни дизйблиться
2) Накидываем OrdinaryPlayerController,BezierCurvePlayerController на нашего персонажа
3) Накидываем ManagerController на нашего персонажа(там выставляются все константы)
4) Накидываем CharacterController на персонажа
5) Помечаем тэгом "Player" нашего персонажа (необходимо для триггеров)

Для кривых, по которым будет двигаться наш персонаж- смотреть описание класса OrdinaryAndBezierCheckoutTrigger

Для того, чтобы объекты двигались по кривой какие-нибудь необходимо добавить им скрипт  BezierCurveMovement или
BezierCurveMovementWithRewind

Для того, чтобы перемотка была у обычных обхъектов с риджит боди: RigidBodyRewind
                                                            без : SimpleRewind

