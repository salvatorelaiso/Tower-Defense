% ===== Input Transforming ===== %
    enemy(ID, X, Y, Health, Type) :-
        enemies(sensors(sensorsDataListsManager(enemies(ID,enemySensorData(health(Health)))))),
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

% Paths
	path(1,2).
	path(2,2).
	path(3,1).
	path(3,2).
	path(4,1).
	path(5,1).
	path(5,2).
	path(6,2).
	path(7,2).

% Others
	start(1,1). end(8,2).

% Adjacents
	adjacent(0,0,0,1).
	adjacent(0,0,1,0).
	adjacent(0,0,1,1).
	adjacent(0,1,0,0).
	adjacent(0,1,0,2).
	adjacent(0,1,1,0).
	adjacent(0,1,1,1).
	adjacent(0,1,1,2).
	adjacent(0,2,0,1).
	adjacent(0,2,0,3).
	adjacent(0,2,1,1).
	adjacent(0,2,1,2).
	adjacent(0,2,1,3).
	adjacent(0,3,0,2).
	adjacent(0,3,1,2).
	adjacent(0,3,1,3).
	adjacent(1,0,1,1).
	adjacent(1,0,2,0).
	adjacent(1,0,2,1).
	adjacent(1,1,1,0).
	adjacent(1,1,1,2).
	adjacent(1,1,2,0).
	adjacent(1,1,2,1).
	adjacent(1,1,2,2).
	adjacent(1,2,1,1).
	adjacent(1,2,1,3).
	adjacent(1,2,2,1).
	adjacent(1,2,2,2).
	adjacent(1,2,2,3).
	adjacent(1,3,1,2).
	adjacent(1,3,2,2).
	adjacent(1,3,2,3).
	adjacent(2,0,1,0).
	adjacent(2,0,1,1).
	adjacent(2,0,2,1).
	adjacent(2,0,3,0).
	adjacent(2,0,3,1).
	adjacent(2,1,1,0).
	adjacent(2,1,1,1).
	adjacent(2,1,1,2).
	adjacent(2,1,2,0).
	adjacent(2,1,2,2).
	adjacent(2,1,3,0).
	adjacent(2,1,3,1).
	adjacent(2,1,3,2).
	adjacent(2,2,1,1).
	adjacent(2,2,1,2).
	adjacent(2,2,1,3).
	adjacent(2,2,2,1).
	adjacent(2,2,2,3).
	adjacent(2,2,3,1).
	adjacent(2,2,3,2).
	adjacent(2,2,3,3).
	adjacent(2,3,1,2).
	adjacent(2,3,1,3).
	adjacent(2,3,2,2).
	adjacent(2,3,3,2).
	adjacent(2,3,3,3).
	adjacent(3,0,2,0).
	adjacent(3,0,2,1).
	adjacent(3,0,3,1).
	adjacent(3,0,4,0).
	adjacent(3,0,4,1).
	adjacent(3,1,2,0).
	adjacent(3,1,2,1).
	adjacent(3,1,2,2).
	adjacent(3,1,3,0).
	adjacent(3,1,3,2).
	adjacent(3,1,4,0).
	adjacent(3,1,4,1).
	adjacent(3,1,4,2).
	adjacent(3,2,2,1).
	adjacent(3,2,2,2).
	adjacent(3,2,2,3).
	adjacent(3,2,3,1).
	adjacent(3,2,3,3).
	adjacent(3,2,4,1).
	adjacent(3,2,4,2).
	adjacent(3,2,4,3).
	adjacent(3,3,2,2).
	adjacent(3,3,2,3).
	adjacent(3,3,3,2).
	adjacent(3,3,4,2).
	adjacent(3,3,4,3).
	adjacent(4,0,3,0).
	adjacent(4,0,3,1).
	adjacent(4,0,4,1).
	adjacent(4,0,5,0).
	adjacent(4,0,5,1).
	adjacent(4,1,3,0).
	adjacent(4,1,3,1).
	adjacent(4,1,3,2).
	adjacent(4,1,4,0).
	adjacent(4,1,4,2).
	adjacent(4,1,5,0).
	adjacent(4,1,5,1).
	adjacent(4,1,5,2).
	adjacent(4,2,3,1).
	adjacent(4,2,3,2).
	adjacent(4,2,3,3).
	adjacent(4,2,4,1).
	adjacent(4,2,4,3).
	adjacent(4,2,5,1).
	adjacent(4,2,5,2).
	adjacent(4,2,5,3).
	adjacent(4,3,3,2).
	adjacent(4,3,3,3).
	adjacent(4,3,4,2).
	adjacent(4,3,5,2).
	adjacent(4,3,5,3).
	adjacent(5,0,4,0).
	adjacent(5,0,4,1).
	adjacent(5,0,5,1).
	adjacent(5,0,6,0).
	adjacent(5,0,6,1).
	adjacent(5,1,4,0).
	adjacent(5,1,4,1).
	adjacent(5,1,4,2).
	adjacent(5,1,5,0).
	adjacent(5,1,5,2).
	adjacent(5,1,6,0).
	adjacent(5,1,6,1).
	adjacent(5,1,6,2).
	adjacent(5,2,4,1).
	adjacent(5,2,4,2).
	adjacent(5,2,4,3).
	adjacent(5,2,5,1).
	adjacent(5,2,5,3).
	adjacent(5,2,6,1).
	adjacent(5,2,6,2).
	adjacent(5,2,6,3).
	adjacent(5,3,4,2).
	adjacent(5,3,4,3).
	adjacent(5,3,5,2).
	adjacent(5,3,6,2).
	adjacent(5,3,6,3).
	adjacent(6,0,5,0).
	adjacent(6,0,5,1).
	adjacent(6,0,6,1).
	adjacent(6,0,7,0).
	adjacent(6,0,7,1).
	adjacent(6,1,5,0).
	adjacent(6,1,5,1).
	adjacent(6,1,5,2).
	adjacent(6,1,6,0).
	adjacent(6,1,6,2).
	adjacent(6,1,7,0).
	adjacent(6,1,7,1).
	adjacent(6,1,7,2).
	adjacent(6,2,5,1).
	adjacent(6,2,5,2).
	adjacent(6,2,5,3).
	adjacent(6,2,6,1).
	adjacent(6,2,6,3).
	adjacent(6,2,7,1).
	adjacent(6,2,7,2).
	adjacent(6,2,7,3).
	adjacent(6,3,5,2).
	adjacent(6,3,5,3).
	adjacent(6,3,6,2).
	adjacent(6,3,7,2).
	adjacent(6,3,7,3).
	adjacent(7,0,6,0).
	adjacent(7,0,6,1).
	adjacent(7,0,7,1).
	adjacent(7,0,8,0).
	adjacent(7,0,8,1).
	adjacent(7,1,6,0).
	adjacent(7,1,6,1).
	adjacent(7,1,6,2).
	adjacent(7,1,7,0).
	adjacent(7,1,7,2).
	adjacent(7,1,8,0).
	adjacent(7,1,8,1).
	adjacent(7,1,8,2).
	adjacent(7,2,6,1).
	adjacent(7,2,6,2).
	adjacent(7,2,6,3).
	adjacent(7,2,7,1).
	adjacent(7,2,7,3).
	adjacent(7,2,8,1).
	adjacent(7,2,8,2).
	adjacent(7,2,8,3).
	adjacent(7,3,6,2).
	adjacent(7,3,6,3).
	adjacent(7,3,7,2).
	adjacent(7,3,8,2).
	adjacent(7,3,8,3).
	adjacent(8,0,7,0).
	adjacent(8,0,7,1).
	adjacent(8,0,8,1).
	adjacent(8,0,9,0).
	adjacent(8,0,9,1).
	adjacent(8,1,7,0).
	adjacent(8,1,7,1).
	adjacent(8,1,7,2).
	adjacent(8,1,8,0).
	adjacent(8,1,8,2).
	adjacent(8,1,9,0).
	adjacent(8,1,9,1).
	adjacent(8,1,9,2).
	adjacent(8,2,7,1).
	adjacent(8,2,7,2).
	adjacent(8,2,7,3).
	adjacent(8,2,8,1).
	adjacent(8,2,8,3).
	adjacent(8,2,9,1).
	adjacent(8,2,9,2).
	adjacent(8,2,9,3).
	adjacent(8,3,7,2).
	adjacent(8,3,7,3).
	adjacent(8,3,8,2).
	adjacent(8,3,9,2).
	adjacent(8,3,9,3).
	adjacent(9,0,8,0).
	adjacent(9,0,8,1).
	adjacent(9,0,9,1).
	adjacent(9,1,8,0).
	adjacent(9,1,8,1).
	adjacent(9,1,8,2).
	adjacent(9,1,9,0).
	adjacent(9,1,9,2).
	adjacent(9,2,8,1).
	adjacent(9,2,8,2).
	adjacent(9,2,8,3).
	adjacent(9,2,9,1).
	adjacent(9,2,9,3).
	adjacent(9,3,8,2).
	adjacent(9,3,8,3).
	adjacent(9,3,9,2).
