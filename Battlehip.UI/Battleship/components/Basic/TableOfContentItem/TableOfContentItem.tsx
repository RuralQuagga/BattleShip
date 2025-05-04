import { View, Text, StyleSheet } from 'react-native';

type TableOfContentItemProps = {
  title: string;
  value: number;
};

export const TableOfContentItem = (props: TableOfContentItemProps) => {
  return (
    <View style={style.container}>
      <Text style={style.title}>{props.title}</Text>
      <Text style={style.number}>{props.value}</Text>
    </View>
  );
};

const style = StyleSheet.create({
  container: {
    flexDirection: 'row',
    justifyContent: 'space-between',
    padding: 10,
    width: '80%',
    borderBottomWidth: 1,
    borderBottomColor: '#002C4C',
  },
  title: {
    fontSize: 20,
    fontWeight: 600,
    fontFamily: 'sans-serif-medium',
    color: '#002C4C',
  },
  number: {
    fontSize: 20,
    fontWeight: 600,
    fontFamily: 'sans-serif-medium',
    color: '#002C4C',    
  },
});
