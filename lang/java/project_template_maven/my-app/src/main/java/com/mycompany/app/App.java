package com.mycompany.app;

import org.json.JSONObject;

/**
 * Hello world!
 *
 */
public class App 
{
    public static void main( String[] args )
    {
        System.out.println( "Hello World!" );

        JSONObject jo = new JSONObject("{ \"abc\" : \"def\" }");
        System.out.println(jo);
    }
}
