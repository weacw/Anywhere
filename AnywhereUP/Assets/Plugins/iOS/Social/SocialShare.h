//
//  SocialShare.h
//  UnityBridgeShare
//
//  Created by NSWell on 2018/4/8.
//  Copyright © 2018年 NSWell. All rights reserved.
//

#import <UIKit/UIKit.h>
@interface SocialShare :NSObject
+(id) shareInstance;
-(void) nativeShare:(NSString*)text media:(NSString*)media;
@end
@interface DataConvertor:NSObject
+ (NSString*) charToNSString: (char*)value;
+ (const char*) NSIntToChar: (NSInteger) value;
+ (const char*) NSStringToChar: (NSString*) value;
+ (NSArray*) charToNSArray: (char*)value;

+ (const char*) serializeErrorWithData:(NSString*)description code: (int) code;
+ (const char*) serializeError:(NSError*)error;

+ (NSMutableString*) serializeErrorWithDataToNSString:(NSString*)description code: (int) code;
+ (NSMutableString*) serializeErrorToNSString:(NSError*)error;


+ (const char*) NSStringsArrayToChar:(NSArray*) array;
+ (NSString*) serializeNSStringsArray:(NSArray*) array;  
@end
