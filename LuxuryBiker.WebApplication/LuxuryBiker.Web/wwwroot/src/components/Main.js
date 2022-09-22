import React from 'react';
import $ from 'jquery';

const Main = ({ children, center }) => {
  let classes = `Main ${center ? 'Main--center': ''}`
  $("body").removeClass("bg-gradient-primary");
  return (
    <main className={classes} >
      {children}
    </main>
  );
};

export default Main;