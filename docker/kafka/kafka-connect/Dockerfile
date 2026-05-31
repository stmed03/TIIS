FROM confluentinc/cp-kafka-connect-base:7.7.7

# Elasticsearch Sink Connector
COPY confluentinc-kafka-connect-elasticsearch-15.1.2 /usr/share/confluent-hub-components/confluentinc-kafka-connect-elasticsearch-15.1.2

# Debezium MySQL Connector
COPY debezium-connector-mysql /usr/share/confluent-hub-components/debezium-connector-mysql

ENV CONNECT_PLUGIN_PATH="/usr/share/java,/usr/share/confluent-hub-components"