Objectif du jeu
Le jeu n’a pas d’objectif fixe. Il s’agit simplement de gérer son zoo en faisant le plus d’argent possible.
Déroulement d’une partie
Le joueur commence avec 4 enclos, 100$ en poches et un personnage contrôlable.
Personnage contrôlable :
Il pourra se déplacer dans le zoo avec les croix directionnelles de son clavier et interagir avec les éléments de la carte. 
Actions avec la souris :
Il pourra également effectuer des actions, en utilisant sa souris. Pour effectuer une action, il devra sélectionner une action dans le menu, qui se situera en bas de l’écran de jeu (l’endroit où se situe le menu est à même de changer), puis il devra cliquer sur la case sur laquelle il désire effectuer l’action. Chaque action aura un coût associé à celle-ci.
Voici la liste des actions qui lui sera possible de réaliser :
Ajouter un animal : Permet au joueur d’ajouter un animal dans un enclos, il peut uniquement ajouter un animal dans une case située dans un enclos et dans un enclos qui ne contient que des animaux de la même espèce. Chaque animal aura un coût de base et devra être nourris régulièrement. Si la jauge de faim de l’animal descend à 0, l’animal rentrera dans l’état faim et il faudra le nourrir pour remonter la jauge au maximum. Si l’animal a faim trop longtemps, le joueur se fera donner une contravention. Plus le zoo contiendra des animaux, plus le nombre de visiteurs augmentera.
Engager un concierge : Permet au joueur d’engager un concierge, le concierge apparaîtra sur la case sur laquelle le joueur cliquera. La case ne peut se situer dans un enclos. Le concierge se promènera aléatoirement sur la carte et ramassera tous les déchets qu’il trouvera. À chaque fois qu’il entrera une nouvelle case, il vérifiera s’il y a un déchet sur un des cases adjacentes et le ramassera si oui. Chaque concierge coûtera 2 $ par minutes.
Nourrir un animal : Permet au joueur de nourrir un animal en cliquant sur celui-ci. Cela remplis la jauge de faim de l’animal au maximum. Le coût pour nourrir un animal sera le même, peu importe le niveau de faim de l’animal. 
Ramasser un déchet : Le joueur peut ramasser les déchets sur la carte, à condition qu’il se trouve sur une case adjacente au déchet qu’il souhaite ramasser.
Autres mécaniques :
Déchets : Les visiteurs laissent tomber des déchets de façon aléatoire sur la carte. Le coût du billet de chaque visiteur baisse de 0.10 $ par déchet.
Visiteurs : Les visiteurs paient le prix d’entrée proportionnel au nombre d’animaux présent dans le parc, ils doivent repayer le prix du billet à chaque minute. Le prix peut changer au fil de leur visite.   
Classes
Joueur : Une classe qui représente le joueur. Elle aura comme propriétés; de l’argent (int), une position dans la carte et un tableau contenant les cases adjacentes. Il aura également une série d’image pour le représenter 
Case : Représente les cases de la carte. Chaque case aura la propriété estUnObstacle pour savoir si les personnages peuvent marcher dessus, un booléen pour savoir si elle se trouve dans un enclos, un autre pour savoir si elle contient un déchet, elle aura une image la représentant.
Visiteurs : Une classe qui représente le visiteur. Il aura un nom généré automatiquement, le temps, en seconde, depuis lequel il est dans le parc, une série d’images le représentant et un genre.
Concierge : Classe représentant le concierge. Il aura un nom, une série d’images le représentant et un genre.
Animal : Classe représentant un animal du zoo. Il aura une espèce, un nom choisi par le joueur, un âge (adulte ou bébé), un sexe (mâle ou femelle). Si l’animal est une femelle adulte et qu’il y a un mâle dans l’enclos, celle-ci peut tomber enceinte et accoucher d’un autre animal de la même espèce, au bout d’un certain nombre de mois, dépendamment de l’espèce de l’animal. Chaque animal aura donc un temps d’accouchement en mois.
