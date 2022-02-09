 //add record
   function beforeadd()
    {
 
   if(confirm('Do You Want To Add Record')==true)
    
    return true;
   else
   
   return false;
    
    }
   //update  record
    function beforeupdate()
    {
   if(confirm('Do You want to Update the Record ')==true)
   return true;
   else
   
   return false;
   
    } 
    //delete record
    
    function beforedelete()
    {
   if(confirm('Do You Want to Delete Record')==true)
    return true;
   else
   
   return false;
        }
    
    
    