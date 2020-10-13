% ===== Input ===== %
    %enemies(sensors(sensorsDataListsManager(enemies(X,enemySensorData(healt(V)))))).
    %enemies(sensors(sensorsDataListsManager(enemies(X,enemySensorData(x(V)))))).
    %enemies(sensors(sensorsDataListsManager(enemies(X,enemySensorData(y(V)))))).
    %nodes(sensors(sensorsDataListsManager(nodes(X,nodeSensorData(turretTypeInt(V)))))).
    %nodes(sensors(sensorsDataListsManager(nodes(X,nodeSensorData(x(V)))))).
    %nodes(sensors(sensorsDataListsManager(nodes(X,nodeSensorData(y(V)))))).
    %player(gameMaster(playerStats(money(X)))).
    %player(gameMaster(playerStats(lives(X)))).


% Directives not supported in dlv2
    % #const none = 0.
    % #const standardTurret = 1.
    % #const standardTurretUpgrded = 2.
    % #const missileLauncher = 3.
    % #const missileLauncherUpgraded = 4.
    % #const laserBeamer = 5.
    % #const laserBeamerUpgraded = 6.


% ===== Input Transforming ===== %
    enemy(ID, X, Y, Healt) :-
        enemies(sensors(sensorsDataListsManager(enemies(ID,enemySensorData(healt(Healt)))))),
        enemies(sensors(sensorsDataListsManager(enemies(ID,enemySensorData(x(X)))))),
        enemies(sensors(sensorsDataListsManager(enemies(ID,enemySensorData(y(Y)))))).

    node(ID, X, Y, TurretType) :-
        nodes(sensors(sensorsDataListsManager(nodes(ID,nodeSensorData(turretTypeInt(TurretType)))))),
        nodes(sensors(sensorsDataListsManager(nodes(ID,nodeSensorData(x(X)))))),
        nodes(sensors(sensorsDataListsManager(nodes(ID,nodeSensorData(y(Y)))))).

    money(Value) :- player(gameMaster(playerStats(money(Value)))).


% possibleBuild(X, Y, standardTurret) :- node(_, X, Y, none), money >= Price, cost(standardTurret, Price).
% possibleBuild(X, Y, missileLauncher) :- node(_, X, Y, none), money >= Price, cost(missileLauncher, Price).
% possibleBuild(X, Y, laserBeamer) :- node(_, X, Y, none), money >= Price, cost(laserBeamer, Price).
possibleBuild(X, Y, 1) :- node(_, X, Y, 0), money >= Price, cost(1, Price).
possibleBuild(X, Y, 3) :- node(_, X, Y, 0), money >= Price, cost(3, Price).
possibleBuild(X, Y, 5) :- node(_, X, Y, 0), money >= Price, cost(5, Price).


% possibleBuild(X, Y, standardTurretUpgrded) :- node(_, X, Y, standardTurret), money >= Price, cost(standardTurretUpgraded, Price).
% possibleBuild(X, Y, missileLauncherUpgraded) :- node(_, X, Y, missileLauncher), money >= Price, cost(missileLauncherUpgraded, Price).
% possibleBuild(X, Y, laserBeamerUpgraded) :- node(_, X, Y, laserBeamerUpgraded), money >= Price, cost(laserBeamerUpgraded, Price).
possibleBuild(X, Y, 2) :- node(_, X, Y, 1), money >= Price, cost(2, Price).
possibleBuild(X, Y, 4) :- node(_, X, Y, 3), money >= Price, cost(4, Price).
possibleBuild(X, Y, 6) :- node(_, X, Y, 5), money >= Price, cost(6, Price).

build(X, Y, Turret) | out(X, Y, Turret) :- possibleBuild(X, Y, Turret).
:- build(X, Y, TurretA), build(X, Y, TurretB), TurretA != TurretB.

expense(Money) :- build(_, _, Turret), cost(Turret, Money).

% evaluate to use #optimize instead of the following
:~ money(Amount), #sum{ Price : expense(Price) } = TotalToPay, RemainingMoney = Amount - TotalToPay. [RemainingMoney@2]
:- money(Amount), #sum{ Price : expense(Price) } = TotalToPay, Amount < TotalToPay.

distanceNodePath(NodeID, 1) :- node(NodeID, NodeX, NodeY, _), path(PathX, PathY), adjacent(NodeX, NodeY, PathX, PathY).
distanceNodePath(NodeID, 1) :- node(NodeID, NodeX, NodeY, _), path(PathX, PathY), adjacent(PathX, PathY, NodeX, NodeY).
distanceNodePath(NodeID, Distance) :- node(NodeID, NodeX, NodeY, _), node(AdjacentNodeID, AdjacentNodeX, AdjacentNodeY, _), adjacent(NodeX, NodeY, AdjacentNodeX, AdjacentNodeY), 
        Distance = AdjacentNodeDistance + 1, distanceNodePath(AdjacentNodeID, AdjacentNodeDistance), Distance < MaxDistance, maxDistance(MaxDistance).

minDistanceNodePath(NodeID, MinDistance) :- node(NodeID, _, _, _), #min{ Distance : distanceNodePath(NodeID, Distance) } = MinDistance.

:~ build(X, Y, _), node(NodeID, X, Y, _), minDistanceNodePath(NodeID, Distance). [Distance@1]

setOnActuator(actuator(brain(aI(x(X))))):- build(X, _, _).
setOnActuator(actuator(brain(aI(y(Y))))):- build(_, Y, _).
setOnActuator(actuator(brain(aI(turretTypeInt(Turret))))):- build(_, _, Turret).


% adjacent
    adjacent( 0, 0,  0, 1).
    adjacent( 0, 0,  1, 0).
    adjacent( 0, 0,  1, 1).
    adjacent( 0, 1,  0, 0).
    adjacent( 0, 1,  0, 2).
    adjacent( 0, 1,  1, 0).
    adjacent( 0, 1,  1, 1).
    adjacent( 0, 1,  1, 2).
    adjacent( 0, 2,  0, 1).
    adjacent( 0, 2,  0, 3).
    adjacent( 0, 2,  1, 1).
    adjacent( 0, 2,  1, 2).
    adjacent( 0, 2,  1, 3).
    adjacent( 0, 3,  0, 2).
    adjacent( 0, 3,  0, 4).
    adjacent( 0, 3,  1, 2).
    adjacent( 0, 3,  1, 3).
    adjacent( 0, 3,  1, 4).
    adjacent( 0, 4,  0, 3).
    adjacent( 0, 4,  0, 5).
    adjacent( 0, 4,  1, 3).
    adjacent( 0, 4,  1, 4).
    adjacent( 0, 4,  1, 5).
    adjacent( 0, 5,  0, 4).
    adjacent( 0, 5,  0, 6).
    adjacent( 0, 5,  1, 4).
    adjacent( 0, 5,  1, 5).
    adjacent( 0, 5,  1, 6).
    adjacent( 0, 6,  0, 5).
    adjacent( 0, 6,  0, 7).
    adjacent( 0, 6,  1, 5).
    adjacent( 0, 6,  1, 6).
    adjacent( 0, 6,  1, 7).
    adjacent( 0, 7,  0, 6).
    adjacent( 0, 7,  1, 6).
    adjacent( 0, 7,  1, 7).
    adjacent( 1, 0,  0, 0).
    adjacent( 1, 0,  0, 1).
    adjacent( 1, 0,  1, 1).
    adjacent( 1, 0,  2, 0).
    adjacent( 1, 0,  2, 1).
    adjacent( 1, 1,  0, 0).
    adjacent( 1, 1,  0, 1).
    adjacent( 1, 1,  0, 2).
    adjacent( 1, 1,  1, 0).
    adjacent( 1, 1,  1, 2).
    adjacent( 1, 1,  2, 0).
    adjacent( 1, 1,  2, 1).
    adjacent( 1, 1,  2, 2).
    adjacent( 1, 2,  0, 1).
    adjacent( 1, 2,  0, 2).
    adjacent( 1, 2,  0, 3).
    adjacent( 1, 2,  1, 1).
    adjacent( 1, 2,  1, 3).
    adjacent( 1, 2,  2, 1).
    adjacent( 1, 2,  2, 2).
    adjacent( 1, 2,  2, 3).
    adjacent( 1, 3,  0, 2).
    adjacent( 1, 3,  0, 3).
    adjacent( 1, 3,  0, 4).
    adjacent( 1, 3,  1, 2).
    adjacent( 1, 3,  1, 4).
    adjacent( 1, 3,  2, 2).
    adjacent( 1, 3,  2, 3).
    adjacent( 1, 3,  2, 4).
    adjacent( 1, 4,  0, 3).
    adjacent( 1, 4,  0, 4).
    adjacent( 1, 4,  0, 5).
    adjacent( 1, 4,  1, 3).
    adjacent( 1, 4,  1, 5).
    adjacent( 1, 4,  2, 3).
    adjacent( 1, 4,  2, 4).
    adjacent( 1, 4,  2, 5).
    adjacent( 1, 5,  0, 4).
    adjacent( 1, 5,  0, 5).
    adjacent( 1, 5,  0, 6).
    adjacent( 1, 5,  1, 4).
    adjacent( 1, 5,  1, 6).
    adjacent( 1, 5,  2, 4).
    adjacent( 1, 5,  2, 5).
    adjacent( 1, 5,  2, 6).
    adjacent( 1, 6,  0, 5).
    adjacent( 1, 6,  0, 6).
    adjacent( 1, 6,  0, 7).
    adjacent( 1, 6,  1, 5).
    adjacent( 1, 6,  1, 7).
    adjacent( 1, 6,  2, 5).
    adjacent( 1, 6,  2, 6).
    adjacent( 1, 6,  2, 7).
    adjacent( 1, 7,  0, 6).
    adjacent( 1, 7,  0, 7).
    adjacent( 1, 7,  1, 6).
    adjacent( 1, 7,  2, 6).
    adjacent( 1, 7,  2, 7).
    adjacent( 2, 0,  1, 0).
    adjacent( 2, 0,  1, 1).
    adjacent( 2, 0,  2, 1).
    adjacent( 2, 0,  3, 0).
    adjacent( 2, 0,  3, 1).
    adjacent( 2, 1,  1, 0).
    adjacent( 2, 1,  1, 1).
    adjacent( 2, 1,  1, 2).
    adjacent( 2, 1,  2, 0).
    adjacent( 2, 1,  2, 2).
    adjacent( 2, 1,  3, 0).
    adjacent( 2, 1,  3, 1).
    adjacent( 2, 1,  3, 2).
    adjacent( 2, 2,  1, 1).
    adjacent( 2, 2,  1, 2).
    adjacent( 2, 2,  1, 3).
    adjacent( 2, 2,  2, 1).
    adjacent( 2, 2,  2, 3).
    adjacent( 2, 2,  3, 1).
    adjacent( 2, 2,  3, 2).
    adjacent( 2, 2,  3, 3).
    adjacent( 2, 3,  1, 2).
    adjacent( 2, 3,  1, 3).
    adjacent( 2, 3,  1, 4).
    adjacent( 2, 3,  2, 2).
    adjacent( 2, 3,  2, 4).
    adjacent( 2, 3,  3, 2).
    adjacent( 2, 3,  3, 3).
    adjacent( 2, 3,  3, 4).
    adjacent( 2, 4,  1, 3).
    adjacent( 2, 4,  1, 4).
    adjacent( 2, 4,  1, 5).
    adjacent( 2, 4,  2, 3).
    adjacent( 2, 4,  2, 5).
    adjacent( 2, 4,  3, 3).
    adjacent( 2, 4,  3, 4).
    adjacent( 2, 4,  3, 5).
    adjacent( 2, 5,  1, 4).
    adjacent( 2, 5,  1, 5).
    adjacent( 2, 5,  1, 6).
    adjacent( 2, 5,  2, 4).
    adjacent( 2, 5,  2, 6).
    adjacent( 2, 5,  3, 4).
    adjacent( 2, 5,  3, 5).
    adjacent( 2, 5,  3, 6).
    adjacent( 2, 6,  1, 5).
    adjacent( 2, 6,  1, 6).
    adjacent( 2, 6,  1, 7).
    adjacent( 2, 6,  2, 5).
    adjacent( 2, 6,  2, 7).
    adjacent( 2, 6,  3, 5).
    adjacent( 2, 6,  3, 6).
    adjacent( 2, 6,  3, 7).
    adjacent( 2, 7,  1, 6).
    adjacent( 2, 7,  1, 7).
    adjacent( 2, 7,  2, 6).
    adjacent( 2, 7,  3, 6).
    adjacent( 2, 7,  3, 7).
    adjacent( 3, 0,  2, 0).
    adjacent( 3, 0,  2, 1).
    adjacent( 3, 0,  3, 1).
    adjacent( 3, 0,  4, 0).
    adjacent( 3, 0,  4, 1).
    adjacent( 3, 1,  2, 0).
    adjacent( 3, 1,  2, 1).
    adjacent( 3, 1,  2, 2).
    adjacent( 3, 1,  3, 0).
    adjacent( 3, 1,  3, 2).
    adjacent( 3, 1,  4, 0).
    adjacent( 3, 1,  4, 1).
    adjacent( 3, 1,  4, 2).
    adjacent( 3, 2,  2, 1).
    adjacent( 3, 2,  2, 2).
    adjacent( 3, 2,  2, 3).
    adjacent( 3, 2,  3, 1).
    adjacent( 3, 2,  3, 3).
    adjacent( 3, 2,  4, 1).
    adjacent( 3, 2,  4, 2).
    adjacent( 3, 2,  4, 3).
    adjacent( 3, 3,  2, 2).
    adjacent( 3, 3,  2, 3).
    adjacent( 3, 3,  2, 4).
    adjacent( 3, 3,  3, 2).
    adjacent( 3, 3,  3, 4).
    adjacent( 3, 3,  4, 2).
    adjacent( 3, 3,  4, 3).
    adjacent( 3, 3,  4, 4).
    adjacent( 3, 4,  2, 3).
    adjacent( 3, 4,  2, 4).
    adjacent( 3, 4,  2, 5).
    adjacent( 3, 4,  3, 3).
    adjacent( 3, 4,  3, 5).
    adjacent( 3, 4,  4, 3).
    adjacent( 3, 4,  4, 4).
    adjacent( 3, 4,  4, 5).
    adjacent( 3, 5,  2, 4).
    adjacent( 3, 5,  2, 5).
    adjacent( 3, 5,  2, 6).
    adjacent( 3, 5,  3, 4).
    adjacent( 3, 5,  3, 6).
    adjacent( 3, 5,  4, 4).
    adjacent( 3, 5,  4, 5).
    adjacent( 3, 5,  4, 6).
    adjacent( 3, 6,  2, 5).
    adjacent( 3, 6,  2, 6).
    adjacent( 3, 6,  2, 7).
    adjacent( 3, 6,  3, 5).
    adjacent( 3, 6,  3, 7).
    adjacent( 3, 6,  4, 5).
    adjacent( 3, 6,  4, 6).
    adjacent( 3, 6,  4, 7).
    adjacent( 3, 7,  2, 6).
    adjacent( 3, 7,  2, 7).
    adjacent( 3, 7,  3, 6).
    adjacent( 3, 7,  4, 6).
    adjacent( 3, 7,  4, 7).
    adjacent( 4, 0,  3, 0).
    adjacent( 4, 0,  3, 1).
    adjacent( 4, 0,  4, 1).
    adjacent( 4, 0,  5, 0).
    adjacent( 4, 0,  5, 1).
    adjacent( 4, 1,  3, 0).
    adjacent( 4, 1,  3, 1).
    adjacent( 4, 1,  3, 2).
    adjacent( 4, 1,  4, 0).
    adjacent( 4, 1,  4, 2).
    adjacent( 4, 1,  5, 0).
    adjacent( 4, 1,  5, 1).
    adjacent( 4, 1,  5, 2).
    adjacent( 4, 2,  3, 1).
    adjacent( 4, 2,  3, 2).
    adjacent( 4, 2,  3, 3).
    adjacent( 4, 2,  4, 1).
    adjacent( 4, 2,  4, 3).
    adjacent( 4, 2,  5, 1).
    adjacent( 4, 2,  5, 2).
    adjacent( 4, 2,  5, 3).
    adjacent( 4, 3,  3, 2).
    adjacent( 4, 3,  3, 3).
    adjacent( 4, 3,  3, 4).
    adjacent( 4, 3,  4, 2).
    adjacent( 4, 3,  4, 4).
    adjacent( 4, 3,  5, 2).
    adjacent( 4, 3,  5, 3).
    adjacent( 4, 3,  5, 4).
    adjacent( 4, 4,  3, 3).
    adjacent( 4, 4,  3, 4).
    adjacent( 4, 4,  3, 5).
    adjacent( 4, 4,  4, 3).
    adjacent( 4, 4,  4, 5).
    adjacent( 4, 4,  5, 3).
    adjacent( 4, 4,  5, 4).
    adjacent( 4, 4,  5, 5).
    adjacent( 4, 5,  3, 4).
    adjacent( 4, 5,  3, 5).
    adjacent( 4, 5,  3, 6).
    adjacent( 4, 5,  4, 4).
    adjacent( 4, 5,  4, 6).
    adjacent( 4, 5,  5, 4).
    adjacent( 4, 5,  5, 5).
    adjacent( 4, 5,  5, 6).
    adjacent( 4, 6,  3, 5).
    adjacent( 4, 6,  3, 6).
    adjacent( 4, 6,  3, 7).
    adjacent( 4, 6,  4, 5).
    adjacent( 4, 6,  4, 7).
    adjacent( 4, 6,  5, 5).
    adjacent( 4, 6,  5, 6).
    adjacent( 4, 6,  5, 7).
    adjacent( 4, 7,  3, 6).
    adjacent( 4, 7,  3, 7).
    adjacent( 4, 7,  4, 6).
    adjacent( 4, 7,  5, 6).
    adjacent( 4, 7,  5, 7).
    adjacent( 5, 0,  4, 0).
    adjacent( 5, 0,  4, 1).
    adjacent( 5, 0,  5, 1).
    adjacent( 5, 0,  6, 0).
    adjacent( 5, 0,  6, 1).
    adjacent( 5, 1,  4, 0).
    adjacent( 5, 1,  4, 1).
    adjacent( 5, 1,  4, 2).
    adjacent( 5, 1,  5, 0).
    adjacent( 5, 1,  5, 2).
    adjacent( 5, 1,  6, 0).
    adjacent( 5, 1,  6, 1).
    adjacent( 5, 1,  6, 2).
    adjacent( 5, 2,  4, 1).
    adjacent( 5, 2,  4, 2).
    adjacent( 5, 2,  4, 3).
    adjacent( 5, 2,  5, 1).
    adjacent( 5, 2,  5, 3).
    adjacent( 5, 2,  6, 1).
    adjacent( 5, 2,  6, 2).
    adjacent( 5, 2,  6, 3).
    adjacent( 5, 3,  4, 2).
    adjacent( 5, 3,  4, 3).
    adjacent( 5, 3,  4, 4).
    adjacent( 5, 3,  5, 2).
    adjacent( 5, 3,  5, 4).
    adjacent( 5, 3,  6, 2).
    adjacent( 5, 3,  6, 3).
    adjacent( 5, 3,  6, 4).
    adjacent( 5, 4,  4, 3).
    adjacent( 5, 4,  4, 4).
    adjacent( 5, 4,  4, 5).
    adjacent( 5, 4,  5, 3).
    adjacent( 5, 4,  5, 5).
    adjacent( 5, 4,  6, 3).
    adjacent( 5, 4,  6, 4).
    adjacent( 5, 4,  6, 5).
    adjacent( 5, 5,  4, 4).
    adjacent( 5, 5,  4, 5).
    adjacent( 5, 5,  4, 6).
    adjacent( 5, 5,  5, 4).
    adjacent( 5, 5,  5, 6).
    adjacent( 5, 5,  6, 4).
    adjacent( 5, 5,  6, 5).
    adjacent( 5, 5,  6, 6).
    adjacent( 5, 6,  4, 5).
    adjacent( 5, 6,  4, 6).
    adjacent( 5, 6,  4, 7).
    adjacent( 5, 6,  5, 5).
    adjacent( 5, 6,  5, 7).
    adjacent( 5, 6,  6, 5).
    adjacent( 5, 6,  6, 6).
    adjacent( 5, 6,  6, 7).
    adjacent( 5, 7,  4, 6).
    adjacent( 5, 7,  4, 7).
    adjacent( 5, 7,  5, 6).
    adjacent( 5, 7,  6, 6).
    adjacent( 5, 7,  6, 7).
    adjacent( 6, 0,  5, 0).
    adjacent( 6, 0,  5, 1).
    adjacent( 6, 0,  6, 1).
    adjacent( 6, 0,  7, 0).
    adjacent( 6, 0,  7, 1).
    adjacent( 6, 1,  5, 0).
    adjacent( 6, 1,  5, 1).
    adjacent( 6, 1,  5, 2).
    adjacent( 6, 1,  6, 0).
    adjacent( 6, 1,  6, 2).
    adjacent( 6, 1,  7, 0).
    adjacent( 6, 1,  7, 1).
    adjacent( 6, 1,  7, 2).
    adjacent( 6, 2,  5, 1).
    adjacent( 6, 2,  5, 2).
    adjacent( 6, 2,  5, 3).
    adjacent( 6, 2,  6, 1).
    adjacent( 6, 2,  6, 3).
    adjacent( 6, 2,  7, 1).
    adjacent( 6, 2,  7, 2).
    adjacent( 6, 2,  7, 3).
    adjacent( 6, 3,  5, 2).
    adjacent( 6, 3,  5, 3).
    adjacent( 6, 3,  5, 4).
    adjacent( 6, 3,  6, 2).
    adjacent( 6, 3,  6, 4).
    adjacent( 6, 3,  7, 2).
    adjacent( 6, 3,  7, 3).
    adjacent( 6, 3,  7, 4).
    adjacent( 6, 4,  5, 3).
    adjacent( 6, 4,  5, 4).
    adjacent( 6, 4,  5, 5).
    adjacent( 6, 4,  6, 3).
    adjacent( 6, 4,  6, 5).
    adjacent( 6, 4,  7, 3).
    adjacent( 6, 4,  7, 4).
    adjacent( 6, 4,  7, 5).
    adjacent( 6, 5,  5, 4).
    adjacent( 6, 5,  5, 5).
    adjacent( 6, 5,  5, 6).
    adjacent( 6, 5,  6, 4).
    adjacent( 6, 5,  6, 6).
    adjacent( 6, 5,  7, 4).
    adjacent( 6, 5,  7, 5).
    adjacent( 6, 5,  7, 6).
    adjacent( 6, 6,  5, 5).
    adjacent( 6, 6,  5, 6).
    adjacent( 6, 6,  5, 7).
    adjacent( 6, 6,  6, 5).
    adjacent( 6, 6,  6, 7).
    adjacent( 6, 6,  7, 5).
    adjacent( 6, 6,  7, 6).
    adjacent( 6, 6,  7, 7).
    adjacent( 6, 7,  5, 6).
    adjacent( 6, 7,  5, 7).
    adjacent( 6, 7,  6, 6).
    adjacent( 6, 7,  7, 6).
    adjacent( 6, 7,  7, 7).
    adjacent( 7, 0,  6, 0).
    adjacent( 7, 0,  6, 1).
    adjacent( 7, 0,  7, 1).
    adjacent( 7, 0,  8, 0).
    adjacent( 7, 0,  8, 1).
    adjacent( 7, 1,  6, 0).
    adjacent( 7, 1,  6, 1).
    adjacent( 7, 1,  6, 2).
    adjacent( 7, 1,  7, 0).
    adjacent( 7, 1,  7, 2).
    adjacent( 7, 1,  8, 0).
    adjacent( 7, 1,  8, 1).
    adjacent( 7, 1,  8, 2).
    adjacent( 7, 2,  6, 1).
    adjacent( 7, 2,  6, 2).
    adjacent( 7, 2,  6, 3).
    adjacent( 7, 2,  7, 1).
    adjacent( 7, 2,  7, 3).
    adjacent( 7, 2,  8, 1).
    adjacent( 7, 2,  8, 2).
    adjacent( 7, 2,  8, 3).
    adjacent( 7, 3,  6, 2).
    adjacent( 7, 3,  6, 3).
    adjacent( 7, 3,  6, 4).
    adjacent( 7, 3,  7, 2).
    adjacent( 7, 3,  7, 4).
    adjacent( 7, 3,  8, 2).
    adjacent( 7, 3,  8, 3).
    adjacent( 7, 3,  8, 4).
    adjacent( 7, 4,  6, 3).
    adjacent( 7, 4,  6, 4).
    adjacent( 7, 4,  6, 5).
    adjacent( 7, 4,  7, 3).
    adjacent( 7, 4,  7, 5).
    adjacent( 7, 4,  8, 3).
    adjacent( 7, 4,  8, 4).
    adjacent( 7, 4,  8, 5).
    adjacent( 7, 5,  6, 4).
    adjacent( 7, 5,  6, 5).
    adjacent( 7, 5,  6, 6).
    adjacent( 7, 5,  7, 4).
    adjacent( 7, 5,  7, 6).
    adjacent( 7, 5,  8, 4).
    adjacent( 7, 5,  8, 5).
    adjacent( 7, 5,  8, 6).
    adjacent( 7, 6,  6, 5).
    adjacent( 7, 6,  6, 6).
    adjacent( 7, 6,  6, 7).
    adjacent( 7, 6,  7, 5).
    adjacent( 7, 6,  7, 7).
    adjacent( 7, 6,  8, 5).
    adjacent( 7, 6,  8, 6).
    adjacent( 7, 6,  8, 7).
    adjacent( 7, 7,  6, 6).
    adjacent( 7, 7,  6, 7).
    adjacent( 7, 7,  7, 6).
    adjacent( 7, 7,  8, 6).
    adjacent( 7, 7,  8, 7).
    adjacent( 8, 0,  7, 0).
    adjacent( 8, 0,  7, 1).
    adjacent( 8, 0,  8, 1).
    adjacent( 8, 0,  9, 0).
    adjacent( 8, 0,  9, 1).
    adjacent( 8, 1,  7, 0).
    adjacent( 8, 1,  7, 1).
    adjacent( 8, 1,  7, 2).
    adjacent( 8, 1,  8, 0).
    adjacent( 8, 1,  8, 2).
    adjacent( 8, 1,  9, 0).
    adjacent( 8, 1,  9, 1).
    adjacent( 8, 1,  9, 2).
    adjacent( 8, 2,  7, 1).
    adjacent( 8, 2,  7, 2).
    adjacent( 8, 2,  7, 3).
    adjacent( 8, 2,  8, 1).
    adjacent( 8, 2,  8, 3).
    adjacent( 8, 2,  9, 1).
    adjacent( 8, 2,  9, 2).
    adjacent( 8, 2,  9, 3).
    adjacent( 8, 3,  7, 2).
    adjacent( 8, 3,  7, 3).
    adjacent( 8, 3,  7, 4).
    adjacent( 8, 3,  8, 2).
    adjacent( 8, 3,  8, 4).
    adjacent( 8, 3,  9, 2).
    adjacent( 8, 3,  9, 3).
    adjacent( 8, 3,  9, 4).
    adjacent( 8, 4,  7, 3).
    adjacent( 8, 4,  7, 4).
    adjacent( 8, 4,  7, 5).
    adjacent( 8, 4,  8, 3).
    adjacent( 8, 4,  8, 5).
    adjacent( 8, 4,  9, 3).
    adjacent( 8, 4,  9, 4).
    adjacent( 8, 4,  9, 5).
    adjacent( 8, 5,  7, 4).
    adjacent( 8, 5,  7, 5).
    adjacent( 8, 5,  7, 6).
    adjacent( 8, 5,  8, 4).
    adjacent( 8, 5,  8, 6).
    adjacent( 8, 5,  9, 4).
    adjacent( 8, 5,  9, 5).
    adjacent( 8, 5,  9, 6).
    adjacent( 8, 6,  7, 5).
    adjacent( 8, 6,  7, 6).
    adjacent( 8, 6,  7, 7).
    adjacent( 8, 6,  8, 5).
    adjacent( 8, 6,  8, 7).
    adjacent( 8, 6,  9, 5).
    adjacent( 8, 6,  9, 6).
    adjacent( 8, 6,  9, 7).
    adjacent( 8, 7,  7, 6).
    adjacent( 8, 7,  7, 7).
    adjacent( 8, 7,  8, 6).
    adjacent( 8, 7,  9, 6).
    adjacent( 8, 7,  9, 7).
    adjacent( 9, 0,  8, 0).
    adjacent( 9, 0,  8, 1).
    adjacent( 9, 0,  9, 1).
    adjacent( 9, 0, 10, 0).
    adjacent( 9, 0, 10, 1).
    adjacent( 9, 1,  8, 0).
    adjacent( 9, 1,  8, 1).
    adjacent( 9, 1,  8, 2).
    adjacent( 9, 1,  9, 0).
    adjacent( 9, 1,  9, 2).
    adjacent( 9, 1, 10, 0).
    adjacent( 9, 1, 10, 1).
    adjacent( 9, 1, 10, 2).
    adjacent( 9, 2,  8, 1).
    adjacent( 9, 2,  8, 2).
    adjacent( 9, 2,  8, 3).
    adjacent( 9, 2,  9, 1).
    adjacent( 9, 2,  9, 3).
    adjacent( 9, 2, 10, 1).
    adjacent( 9, 2, 10, 2).
    adjacent( 9, 2, 10, 3).
    adjacent( 9, 3,  8, 2).
    adjacent( 9, 3,  8, 3).
    adjacent( 9, 3,  8, 4).
    adjacent( 9, 3,  9, 2).
    adjacent( 9, 3,  9, 4).
    adjacent( 9, 3, 10, 2).
    adjacent( 9, 3, 10, 3).
    adjacent( 9, 3, 10, 4).
    adjacent( 9, 4,  8, 3).
    adjacent( 9, 4,  8, 4).
    adjacent( 9, 4,  8, 5).
    adjacent( 9, 4,  9, 3).
    adjacent( 9, 4,  9, 5).
    adjacent( 9, 4, 10, 3).
    adjacent( 9, 4, 10, 4).
    adjacent( 9, 4, 10, 5).
    adjacent( 9, 5,  8, 4).
    adjacent( 9, 5,  8, 5).
    adjacent( 9, 5,  8, 6).
    adjacent( 9, 5,  9, 4).
    adjacent( 9, 5,  9, 6).
    adjacent( 9, 5, 10, 4).
    adjacent( 9, 5, 10, 5).
    adjacent( 9, 5, 10, 6).
    adjacent( 9, 6,  8, 5).
    adjacent( 9, 6,  8, 6).
    adjacent( 9, 6,  8, 7).
    adjacent( 9, 6,  9, 5).
    adjacent( 9, 6,  9, 7).
    adjacent( 9, 6, 10, 5).
    adjacent( 9, 6, 10, 6).
    adjacent( 9, 6, 10, 7).
    adjacent( 9, 7,  8, 6).
    adjacent( 9, 7,  8, 7).
    adjacent( 9, 7,  9, 6).
    adjacent( 9, 7, 10, 6).
    adjacent( 9, 7, 10, 7).
    adjacent(10, 0,  9, 0).
    adjacent(10, 0,  9, 1).
    adjacent(10, 0, 10, 1).
    adjacent(10, 0, 11, 0).
    adjacent(10, 0, 11, 1).
    adjacent(10, 1,  9, 0).
    adjacent(10, 1,  9, 1).
    adjacent(10, 1,  9, 2).
    adjacent(10, 1, 10, 0).
    adjacent(10, 1, 10, 2).
    adjacent(10, 1, 11, 0).
    adjacent(10, 1, 11, 1).
    adjacent(10, 1, 11, 2).
    adjacent(10, 2,  9, 1).
    adjacent(10, 2,  9, 2).
    adjacent(10, 2,  9, 3).
    adjacent(10, 2, 10, 1).
    adjacent(10, 2, 10, 3).
    adjacent(10, 2, 11, 1).
    adjacent(10, 2, 11, 2).
    adjacent(10, 2, 11, 3).
    adjacent(10, 3,  9, 2).
    adjacent(10, 3,  9, 3).
    adjacent(10, 3,  9, 4).
    adjacent(10, 3, 10, 2).
    adjacent(10, 3, 10, 4).
    adjacent(10, 3, 11, 2).
    adjacent(10, 3, 11, 3).
    adjacent(10, 3, 11, 4).
    adjacent(10, 4,  9, 3).
    adjacent(10, 4,  9, 4).
    adjacent(10, 4,  9, 5).
    adjacent(10, 4, 10, 3).
    adjacent(10, 4, 10, 5).
    adjacent(10, 4, 11, 3).
    adjacent(10, 4, 11, 4).
    adjacent(10, 4, 11, 5).
    adjacent(10, 5,  9, 4).
    adjacent(10, 5,  9, 5).
    adjacent(10, 5,  9, 6).
    adjacent(10, 5, 10, 4).
    adjacent(10, 5, 10, 6).
    adjacent(10, 5, 11, 4).
    adjacent(10, 5, 11, 5).
    adjacent(10, 5, 11, 6).
    adjacent(10, 6,  9, 5).
    adjacent(10, 6,  9, 6).
    adjacent(10, 6,  9, 7).
    adjacent(10, 6, 10, 5).
    adjacent(10, 6, 10, 7).
    adjacent(10, 6, 11, 5).
    adjacent(10, 6, 11, 6).
    adjacent(10, 6, 11, 7).
    adjacent(10, 7,  9, 6).
    adjacent(10, 7,  9, 7).
    adjacent(10, 7, 10, 6).
    adjacent(10, 7, 11, 6).
    adjacent(10, 7, 11, 7).
    adjacent(11, 0, 10, 0).
    adjacent(11, 0, 10, 1).
    adjacent(11, 0, 11, 1).
    adjacent(11, 0, 12, 0).
    adjacent(11, 0, 12, 1).
    adjacent(11, 1, 10, 0).
    adjacent(11, 1, 10, 1).
    adjacent(11, 1, 10, 2).
    adjacent(11, 1, 11, 0).
    adjacent(11, 1, 11, 2).
    adjacent(11, 1, 12, 0).
    adjacent(11, 1, 12, 1).
    adjacent(11, 1, 12, 2).
    adjacent(11, 2, 10, 1).
    adjacent(11, 2, 10, 2).
    adjacent(11, 2, 10, 3).
    adjacent(11, 2, 11, 1).
    adjacent(11, 2, 11, 3).
    adjacent(11, 2, 12, 1).
    adjacent(11, 2, 12, 2).
    adjacent(11, 2, 12, 3).
    adjacent(11, 3, 10, 2).
    adjacent(11, 3, 10, 3).
    adjacent(11, 3, 10, 4).
    adjacent(11, 3, 11, 2).
    adjacent(11, 3, 11, 4).
    adjacent(11, 3, 12, 2).
    adjacent(11, 3, 12, 3).
    adjacent(11, 3, 12, 4).
    adjacent(11, 4, 10, 3).
    adjacent(11, 4, 10, 4).
    adjacent(11, 4, 10, 5).
    adjacent(11, 4, 11, 3).
    adjacent(11, 4, 11, 5).
    adjacent(11, 4, 12, 3).
    adjacent(11, 4, 12, 4).
    adjacent(11, 4, 12, 5).
    adjacent(11, 5, 10, 4).
    adjacent(11, 5, 10, 5).
    adjacent(11, 5, 10, 6).
    adjacent(11, 5, 11, 4).
    adjacent(11, 5, 11, 6).
    adjacent(11, 5, 12, 4).
    adjacent(11, 5, 12, 5).
    adjacent(11, 5, 12, 6).
    adjacent(11, 6, 10, 5).
    adjacent(11, 6, 10, 6).
    adjacent(11, 6, 10, 7).
    adjacent(11, 6, 11, 5).
    adjacent(11, 6, 11, 7).
    adjacent(11, 6, 12, 5).
    adjacent(11, 6, 12, 6).
    adjacent(11, 6, 12, 7).
    adjacent(11, 7, 10, 6).
    adjacent(11, 7, 10, 7).
    adjacent(11, 7, 11, 6).
    adjacent(11, 7, 12, 6).
    adjacent(11, 7, 12, 7).
    adjacent(12, 0, 11, 0).
    adjacent(12, 0, 11, 1).
    adjacent(12, 0, 12, 1).
    adjacent(12, 0, 13, 0).
    adjacent(12, 0, 13, 1).
    adjacent(12, 1, 11, 0).
    adjacent(12, 1, 11, 1).
    adjacent(12, 1, 11, 2).
    adjacent(12, 1, 12, 0).
    adjacent(12, 1, 12, 2).
    adjacent(12, 1, 13, 0).
    adjacent(12, 1, 13, 1).
    adjacent(12, 1, 13, 2).
    adjacent(12, 2, 11, 1).
    adjacent(12, 2, 11, 2).
    adjacent(12, 2, 11, 3).
    adjacent(12, 2, 12, 1).
    adjacent(12, 2, 12, 3).
    adjacent(12, 2, 13, 1).
    adjacent(12, 2, 13, 2).
    adjacent(12, 2, 13, 3).
    adjacent(12, 3, 11, 2).
    adjacent(12, 3, 11, 3).
    adjacent(12, 3, 11, 4).
    adjacent(12, 3, 12, 2).
    adjacent(12, 3, 12, 4).
    adjacent(12, 3, 13, 2).
    adjacent(12, 3, 13, 3).
    adjacent(12, 3, 13, 4).
    adjacent(12, 4, 11, 3).
    adjacent(12, 4, 11, 4).
    adjacent(12, 4, 11, 5).
    adjacent(12, 4, 12, 3).
    adjacent(12, 4, 12, 5).
    adjacent(12, 4, 13, 3).
    adjacent(12, 4, 13, 4).
    adjacent(12, 4, 13, 5).
    adjacent(12, 5, 11, 4).
    adjacent(12, 5, 11, 5).
    adjacent(12, 5, 11, 6).
    adjacent(12, 5, 12, 4).
    adjacent(12, 5, 12, 6).
    adjacent(12, 5, 13, 4).
    adjacent(12, 5, 13, 5).
    adjacent(12, 5, 13, 6).
    adjacent(12, 6, 11, 5).
    adjacent(12, 6, 11, 6).
    adjacent(12, 6, 11, 7).
    adjacent(12, 6, 12, 5).
    adjacent(12, 6, 12, 7).
    adjacent(12, 6, 13, 5).
    adjacent(12, 6, 13, 6).
    adjacent(12, 6, 13, 7).
    adjacent(12, 7, 11, 6).
    adjacent(12, 7, 11, 7).
    adjacent(12, 7, 12, 6).
    adjacent(12, 7, 13, 6).
    adjacent(12, 7, 13, 7).
    adjacent(13, 0, 12, 0).
    adjacent(13, 0, 12, 1).
    adjacent(13, 0, 13, 1).
    adjacent(13, 1, 12, 0).
    adjacent(13, 1, 12, 1).
    adjacent(13, 1, 12, 2).
    adjacent(13, 1, 13, 0).
    adjacent(13, 1, 13, 2).
    adjacent(13, 2, 12, 1).
    adjacent(13, 2, 12, 2).
    adjacent(13, 2, 12, 3).
    adjacent(13, 2, 13, 1).
    adjacent(13, 2, 13, 3).
    adjacent(13, 3, 12, 2).
    adjacent(13, 3, 12, 3).
    adjacent(13, 3, 12, 4).
    adjacent(13, 3, 13, 2).
    adjacent(13, 3, 13, 4).
    adjacent(13, 4, 12, 3).
    adjacent(13, 4, 12, 4).
    adjacent(13, 4, 12, 5).
    adjacent(13, 4, 13, 3).
    adjacent(13, 4, 13, 5).
    adjacent(13, 5, 12, 4).
    adjacent(13, 5, 12, 5).
    adjacent(13, 5, 12, 6).
    adjacent(13, 5, 13, 4).
    adjacent(13, 5, 13, 6).
    adjacent(13, 6, 12, 5).
    adjacent(13, 6, 12, 6).
    adjacent(13, 6, 12, 7).
    adjacent(13, 6, 13, 5).
    adjacent(13, 6, 13, 7).
    adjacent(13, 7, 12, 6).
    adjacent(13, 7, 12, 7).
    adjacent(13, 7, 13, 6).
% path
    path( 1,1).
	path( 2,1).
	path( 3,1).
	path( 3,3).
	path( 3,4).
	path( 3,5).
	path( 3,6).
	path( 4,1).
	path( 4,3).
	path( 4,6).
    path( 5,1).
	path( 5,3).
	path( 5,6).
	path( 6,1).
	path( 6,2).
	path( 6,3).
	path( 6,6).
	path( 7,6).
	path( 8,6).
	path( 9,6).
    path(10,3).
	path(10,4).
	path(10,5).
	path(10,6).
	path(11,3).
	path(12,3).
	path(12,4).
	path(12,5).
	path(12,6).
% cost
    % cost(standardTurret, 100). 
    % cost(standardTurretUpgraded, 60).
    % cost(missileLauncher, 250).
    % cost(missileLauncherUpgraded, 150).
    % cost(laserBeamer, 350).
    % cost(laserBeamerUpgraded, 250).
    cost(1, 100). 
    cost(2, 60).
    cost(3, 250).
    cost(4, 150).
    cost(5, 350).
    cost(6, 250).
% others
    start(1, 1). end(12, 6). maxDistance(14).

node(0, 0, 0, 0).
node(1, 1, 0, 0).
node(2, 2, 0, 1).
money(1000).
