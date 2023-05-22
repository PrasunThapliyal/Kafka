24 Nov 2021
===========

ref: (****) https://www.youtube.com/watch?v=R873BlNVUB4
	Apache Kafka Crash Course (Hussein Nasser)
		
	Follow this to install Kafka and Zookeeper in docker containers on onxv1338
		[root@onxv1338 ~]# docker ps -a | grep zoo
			fbec6c29bd23        artifactory.ciena.com/blueplanet/zookeeper:1.10.4-z3.6.2                   "pid1 zkhooks.zookeep"   2 weeks ago         Up 2 weeks                                                                      zookeeper_1.10.4-z3.6.2_0
		[root@onxv1338 ~]# docker ps -a | grep kaf
			8ab567f468e5        artifactory.ciena.com/blueplanet/kafka:3.5.1-k2.3.0                        "supervisord -c /yeti"   2 weeks ago         Up 2 weeks               8080/tcp, 9092/tcp                                     kafka_3.5.1-k2.3.0_0
		[root@onxv1338 ~]#
		[root@onxv1338 ~]# docker run --name ptZookeeper -p 2181:2181 -d zookeeper
			Digest: sha256:9580eb3dfe20c116cbc3c39a7d9e347d2e34367002e2790af4fac31208e18ec5
			Status: Downloaded newer image for zookeeper:latest
			3d7b6a3c7cf925703b1160ede5ad6e26ccafc1d5ace0ad2441dfc6e6e887870d
		[root@onxv1338 ~]# docker ps -a | grep zoo
			3d7b6a3c7cf9        zookeeper                                                                  "/docker-entrypoint.s"   4 minutes ago       Up 4 minutes             2888/tcp, 3888/tcp, 0.0.0.0:2181->2181/tcp, 8080/tcp   ptZookeeper
			fbec6c29bd23        artifactory.ciena.com/blueplanet/zookeeper:1.10.4-z3.6.2                   "pid1 zkhooks.zookeep"   2 weeks ago         Up 2 weeks                                                                      zookeeper_1.10.4-z3.6.2_0
		[root@onxv1338 ~]# docker run -p 9092:9092 --name ptKafka  -e KAFKA_ZOOKEEPER_CONNECT=onxv1338:2181 -e KAFKA_ADVERTISED_LISTENERS=PLAINTEXT://onxv1338:9092 -e KAFKA_OFFSETS_TOPIC_REPLICATION_FACTOR=1 -d confluentinc/cp-kafka
			Digest: sha256:9b3f922f03bed5bab9cd62df8eaad7fd72d26a8b42d87bfcbde3905a4295ec25
			Status: Downloaded newer image for confluentinc/cp-kafka:latest
			26049fbf4065ea788476b378309fa579465a3d6855d32d7585220edd1862ac50
		[root@onxv1338 ~]#
		[root@onxv1338 ~]# docker ps -a | grep kaf
			26049fbf4065        confluentinc/cp-kafka                                                      "/etc/confluent/docke"   About a minute ago   Up About a minute        0.0.0.0:9092->9092/tcp                                 ptKafka
			8ab567f468e5        artifactory.ciena.com/blueplanet/kafka:3.5.1-k2.3.0                        "supervisord -c /yeti"   2 weeks ago          Up 2 weeks               8080/tcp, 9092/tcp                                     kafka_3.5.1-k2.3.0_0

ref: (****) https://thecloudblog.net/post/building-reliable-kafka-producers-and-consumers-in-net/
	Building Reliable Kafka Producers and Consumers in .NET
		
	Follow this to create a publisher and consumer in dotnet to talk to kafka on onxv1338
		Works out of the box !!

ref: https://thecloudblog.net/post/building-reliable-kafka-producers-and-consumers-in-net/
	Building Reliable Kafka Producers and Consumers in .NET

22 May 2023
===========

Try connecting to the Kafka containers that come with BP2 instead of creating our own new containers
	The big issue is that the BP2 containers do not publish advertized listeners. Instead, they send the IP address of the broker to the client, eg 172.16.0.12

	So for eg if we are creating a consumer, once the consumer gets meta from the broker, it tries connecting to 172.16.0.12 .. which my local windows machine does not know

	Read this for an understaning:	
	ref: (****) My Python/Java/Spring/Go/Whatever Client Won’t Connect to My Apache Kafka Cluster in Docker/AWS/My Brother’s Laptop. Please Help!
		https://www.confluent.io/blog/kafka-client-cannot-connect-to-broker-on-aws-on-docker-etc/

	Step 1:
		From our local system, create a tunnel from localhost:9092 to onxv1339 -> 172.16.0.12:9092
	Step 2:
		Create a Kafka client (This code), set bootstrap server = localhost:9092
	Step 3:
		Add a route to Windows route table to redirect all traffic destined for 172.16.0.12 to the default gateway, 
		and this must be accompanied by another tunnel from 172.16.0.12:9092 (my laptop) to 172.16.0.12:9092 (on onxv1339)
	Step 4:
		Run the client, choose consumer, the topic is "bp.equipmenttopologyplanning.v1.websocketgenericpushtopic", and for this the Key and Value are strings (UTF8)
	----------------------------------------------------
	Some elaborations for above steps.
	Note, I define the tunnels in Steps 1 and 3 in one go, when we reach Step 3. But for understanding, its easier to read steps in order

	Step 2: (ref: https://serverfault.com/questions/712970/can-i-redirect-route-ip-adress-to-another-ip-address-windows)
		CIENA+pthapliy@DESKTOP-V99LATL MINGW64 /c/GIT/MyPersonal/Kafka (master)
		$ netsh int ip sh int

		Idx     Met         MTU          State                Name
		---  ----------  ----------  ------------  ---------------------------
		  1          75  4294967295  connected     Loopback Pseudo-Interface 1
		 16          35        1500  connected     Wi-Fi
		 10          25        1500  disconnected  Local Area Connection* 1
		 14          65        1500  disconnected  Bluetooth Network Connection
		 12          25        1500  disconnected  Local Area Connection* 2


		CIENA+pthapliy@DESKTOP-V99LATL MINGW64 /c/GIT/MyPersonal/Kafka (master)
		$ netsh int ip add addr 1 172.16.0.12/32 st=ac sk=tr


		CIENA+pthapliy@DESKTOP-V99LATL MINGW64 /c/GIT/MyPersonal/Kafka (master)
		$ tracert 172.16.0.12

		Tracing route to DESKTOP-V99LATL.ciena.com [172.16.0.12]
		over a maximum of 30 hops:

		  1    <1 ms    <1 ms    <1 ms  DESKTOP-V99LATL.ciena.com [172.16.0.12]

		Trace complete.

		CIENA+pthapliy@DESKTOP-V99LATL MINGW64 /c/GIT/MyPersonal/Kafka (master)
		$ netsh int ip delete  addr 1 172.16.0.12

	Steps 1 and 3:
		CIENA+pthapliy@DESKTOP-V99LATL MINGW64 ~
		$ ssh root@onxv1339.ott.ciena.com -CNL localhost:9092:172.16.0.12:9092 -CNL 172.16.0.12:9092:172.16.0.12:9092
		root@onxv1339.ott.ciena.com's password:


