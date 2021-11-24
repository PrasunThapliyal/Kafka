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

