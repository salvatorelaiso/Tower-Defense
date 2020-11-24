% ===== Input Transforming ===== %
    enemy(ID, X, Y, Healt, Type) :-
        enemies(sensors(sensorsDataListsManager(enemies(ID,enemySensorData(healt(Healt)))))),
        enemies(sensors(sensorsDataListsManager(enemies(ID,enemySensorData(type(Type)))))),
        enemies(sensors(sensorsDataListsManager(enemies(ID,enemySensorData(x(X)))))),
        enemies(sensors(sensorsDataListsManager(enemies(ID,enemySensorData(y(Y)))))).

    node(ID, X, Y, TurretType) :-
        nodes(sensors(sensorsDataListsManager(nodes(ID,nodeSensorData(turretTypeName(TurretType)))))),
        nodes(sensors(sensorsDataListsManager(nodes(ID,nodeSensorData(x(X)))))),
        nodes(sensors(sensorsDataListsManager(nodes(ID,nodeSensorData(y(Y)))))).

    money(Value) :- player(gameMaster(playerStats(money(Value)))).

% Do not build if there is no enemy
:- #count{ID : enemy(ID, _, _, _, _)} = 0.

% List possible builds
possibleBuild(X, Y, standardTurret) :- node(_, X, Y, none), money >= Price, cost(standardTurret, Price), money(Money).
possibleBuild(X, Y, missileLauncher) :- node(_, X, Y, none), money >= Price, cost(missileLauncher, Price), money(Money).
possibleBuild(X, Y, laserBeamer) :- node(_, X, Y, none), money >= Price, cost(laserBeamer, Price), money(Money).

% List possible upgrades
possibleBuild(X, Y, standardTurretUpgraded) :- node(_, X, Y, standardTurret), money >= Price, cost(standardTurretUpgraded, Price), money(Money).
possibleBuild(X, Y, missileLauncherUpgraded) :- node(_, X, Y, missileLauncher), money >= Price, cost(missileLauncherUpgraded, Price), money(Money).
possibleBuild(X, Y, laserBeamerUpgraded) :- node(_, X, Y, laserBeamerUpgraded), money >= Price, cost(laserBeamerUpgraded, Price), money(Money).

% Generate all possible plans, excluding multiple builds in the same position
build(X, Y, Turret) | out(X, Y, Turret) :- possibleBuild(X, Y, Turret).
:- build(X, Y, TurretA), build(X, Y, TurretB), TurretA != TurretB.

% Ensure the total amount of money necessary doesn't exceed the player's actual money
expense(Money) :- build(_, _, Turret), cost(Turret, Money).
:- money(Amount), #sum{ Price : expense(Price) } = TotalToPay, Amount < TotalToPay.

% Maximize the expense
:~ money(Amount), #sum{ Price : expense(Price) } = TotalToPay, RemainingMoney = Amount - TotalToPay. [RemainingMoney@2]

:- #count{ID : node(ID, _, _, Turret), Turret != none} = Turrets, build(X, Y, _), end(EndX, EndY), not adjacent(X, Y, EndX, EndY), Turrets = 0.
nodePositionCoefficient(NodeX, NodeY, Value) :-
	node(_, NodeX, NodeY, _),
	#count{ X, Y : adjacent(NodeX, NodeY, X, Y), path(X, Y)} = Paths,
	#count{ X, Y : adjacent(NodeX, NodeY, X, Y), node(_, X, Y, Turret), Turret != none} = NotEmptyNodes,
	Value = Paths + NotEmptyNodes*2.
:~ nodePositionCoefficient(NodeX, NodeY, Value), build(NodeX, NodeY, _), AmountToPay = 16 - Value. [AmountToPay@1]

% Take only one action from the plan to put it in the actuator
action(X, Y, Turret) | out(X, Y, Turret) :- build(X, Y, Turret).
:- #count{X, Y, Turret : action(X, Y, Turret)} > 1.

setOnActuator(actuator(brain(aI(x(X))))):- build(X, _, _).
setOnActuator(actuator(brain(aI(y(Y))))):- build(_, Y, _).
setOnActuator(actuator(brain(aI(turretTypeName(Turret))))):- build(_, _, Turret).


% Costs
    cost(standardTurret, 100). 
    cost(standardTurretUpgraded, 60).
    cost(missileLauncher, 250).
    cost(missileLauncherUpgraded, 150).
    cost(laserBeamer, 350).
    cost(laserBeamerUpgraded, 250).
