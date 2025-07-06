import { ScrollView, ViewStyle, StyleSheet } from 'react-native';

type ScrollViewProps = {
  children: React.ReactNode;
  contentContainerStyle?: ViewStyle;
};

export const CustomScrollView = (props: ScrollViewProps) => {
  return (
    <ScrollView
      style={styles.scrollView}
      contentContainerStyle={props.contentContainerStyle}
    >
      {props.children}
    </ScrollView>
  );
};

const styles = StyleSheet.create({
  scrollView: {
    flex: 1,    
  },
});
